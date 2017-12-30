using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using OsuHelper.Exceptions;
using OsuHelper.Models;
using Polly;
using Tyrrrz.Extensions;

namespace OsuHelper.Services
{
    public class DataService : IDataService, IDisposable
    {
        private readonly ISettingsService _settingsService;
        private readonly ICacheService _cacheService;

        private readonly HttpClient _httpClient;
        private readonly SemaphoreSlim _requestRateSemaphore;
        private readonly Policy _requestPolicy;

        private DateTime _lastRequestDateTime;

        private string ApiKey => _settingsService.ApiKey;

        public DataService(ISettingsService settingsService, ICacheService cacheService)
        {
            _settingsService = settingsService;
            _cacheService = cacheService;

            // Connection limit
            ServicePointManager.DefaultConnectionLimit = 9999;

            // Client
            var handler = new HttpClientHandler();
            if (handler.SupportsAutomaticDecompression)
                handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            _httpClient = new HttpClient(handler, true);
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "osu!helper (github.com/Tyrrrz/OsuHelper)");

            // Rate limiting
            _requestRateSemaphore = new SemaphoreSlim(1, 1);
            _lastRequestDateTime = DateTime.MinValue;

            // Request policy
            _requestPolicy = Policy
                .Handle<HttpErrorStatusCodeException>(ex => ex.StatusCode == HttpStatusCode.InternalServerError)
                .Or<HttpRequestException>(ex => ex.InnerException is IOException) // sometimes osu web server closes connection randomly
                .WaitAndRetryAsync(5, _ => TimeSpan.FromSeconds(1));
        }

        private async Task MaintainRateLimitAsync(TimeSpan interval)
        {
            await _requestRateSemaphore.WaitAsync();
            var timePassedSinceLastRequest = DateTime.Now - _lastRequestDateTime;
            var remainingTime = interval - timePassedSinceLastRequest;
            if (remainingTime > TimeSpan.Zero)
                await Task.Delay(remainingTime);
            _lastRequestDateTime = DateTime.Now;
            _requestRateSemaphore.Release();
        }

        private async Task<HttpResponseMessage> InternalSendRequestAsync(HttpRequestMessage request)
        {
            // Rate limiting
            await MaintainRateLimitAsync(TimeSpan.FromMinutes(1.0 / 1200));

            // Get response
            var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            // Check status code
            // We throw our own exception here because default one doesn't have status code
            if (!response.IsSuccessStatusCode)
                throw new HttpErrorStatusCodeException(response.StatusCode);

            // Get content
            return response;
        }

        private async Task<string> GetStringAsync(string url)
        {
            return await _requestPolicy.ExecuteAsync(async () =>
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, url))
                using (var response = await InternalSendRequestAsync(request))
                    return await response.Content.ReadAsStringAsync();
            });
        }

        private async Task<Stream> GetStreamAsync(string url)
        {
            return await _requestPolicy.ExecuteAsync(async () =>
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, url))
                {
                    var response = await InternalSendRequestAsync(request);
                    return await response.Content.ReadAsStreamAsync();
                }
            });
        }

        public async Task<Beatmap> GetBeatmapAsync(string beatmapId, GameMode gameMode)
        {
            // Try to get from cache first
            var cached = _cacheService.RetrieveOrDefault<Beatmap>($"Beatmap-{beatmapId}");
            if (cached != null) return cached;

            // Get
            var url = $"https://osu.ppy.sh/api/get_beatmaps?k={ApiKey}&m={(int) gameMode}&b={beatmapId}&limit=1&a=1";
            var response = await GetStringAsync(url);

            // Parse
            var beatmapJson = JToken.Parse(response).First;

            // Extract data
            var id = beatmapJson["beatmap_id"].Value<string>();
            var setId = beatmapJson["beatmapset_id"].Value<string>();
            var creator = beatmapJson["creator"].Value<string>();
            var lastUpdate = beatmapJson["last_update"].Value<DateTime>();
            var artist = beatmapJson["artist"].Value<string>();
            var title = beatmapJson["title"].Value<string>();
            var version = beatmapJson["version"].Value<string>();
            var maxCombo = beatmapJson["max_combo"].Value<int?>() ?? 0; // can be null sometimes
            var duration = TimeSpan.FromSeconds(beatmapJson["hit_length"].Value<double>());
            var bpm = beatmapJson["bpm"].Value<double>();
            var sr = beatmapJson["difficultyrating"].Value<double>();
            var ar = beatmapJson["diff_approach"].Value<double>();
            var od = beatmapJson["diff_overall"].Value<double>();
            var cs = beatmapJson["diff_size"].Value<double>();
            var hp = beatmapJson["diff_drain"].Value<double>();

            var traits = new BeatmapTraits(maxCombo, duration, bpm, sr, ar, od, cs, hp);
            var result = new Beatmap(id, setId, gameMode, creator, lastUpdate, artist, title, version, traits);

            // Save to cache
            _cacheService.Store($"Beatmap-{beatmapId}", result);

            return result;
        }

        public async Task<string> GetBeatmapRawAsync(string beatmapId)
        {
            // Try get from cache first
            var cached = _cacheService.RetrieveOrDefault<string>($"BeatmapRaw-{beatmapId}");
            if (cached != null) return cached;

            // Get
            var url = $"https://osu.ppy.sh/osu/{beatmapId}";
            var response = await GetStringAsync(url);

            // Save to cache
            _cacheService.Store($"BeatmapRaw-{beatmapId}", response);

            return response;
        }

        public async Task<Stream> GetMapSetPreviewAsync(string mapSetId)
        {
            // Try get from cache first
            var cached = _cacheService.RetrieveOrDefault<Stream>($"BeatmapPreview-{mapSetId}");
            if (cached != null) return cached;

            // Get
            var url = $"https://b.ppy.sh/preview/{mapSetId}.mp3";
            var response = await GetStreamAsync(url);

            // Save to cache
            _cacheService.Store($"BeatmapPreview-{mapSetId}", response);

            // Load from cache because this stream cannot seek
            return _cacheService.RetrieveOrDefault<Stream>($"BeatmapPreview-{mapSetId}");
        }

        public async Task<IReadOnlyList<Play>> GetUserTopPlaysAsync(string userId, GameMode gameMode)
        {
            // Don't cache volatile data

            // Get
            var url =
                $"https://osu.ppy.sh/api/get_user_best?k={ApiKey}&m={(int) gameMode}&u={userId.UrlEncode()}&limit=100";
            var response = await GetStringAsync(url);

            // Parse
            var playsJson = JToken.Parse(response);

            // Extract data
            var result = new List<Play>();
            foreach (var playJson in playsJson)
            {
                var playerId = playJson["user_id"].Value<string>();
                var mapId = playJson["beatmap_id"].Value<string>();
                var mods = (Mods) playJson["enabled_mods"].Value<int>();
                var rank = playJson["rank"].Value<string>().ParseEnum<PlayRank>();
                var combo = playJson["maxcombo"].Value<int>();
                var count300 = playJson["count300"].Value<int>();
                var count100 = playJson["count100"].Value<int>();
                var count50 = playJson["count50"].Value<int>();
                var countMiss = playJson["countmiss"].Value<int>();
                var pp = playJson["pp"].Value<double>();

                var play = new Play(playerId, mapId, mods, rank, combo, count300, count100, count50, countMiss, pp);
                result.Add(play);
            }

            return result;
        }

        public async Task<IReadOnlyList<Play>> GetBeatmapTopPlaysAsync(string beatmapId, GameMode gameMode, Mods mods)
        {
            // Don't cache volatile data

            // Get
            var url = $"https://osu.ppy.sh/api/get_scores?k={ApiKey}&m={(int) gameMode}&b={beatmapId}&limit=100";
            var response = await GetStringAsync(url);

            // Parse
            var playsJson = JToken.Parse(response);

            // Extract data
            var result = new List<Play>();
            foreach (var playJson in playsJson)
            {
                var playerId = playJson["user_id"].Value<string>();
                var rank = playJson["rank"].Value<string>().ParseEnum<PlayRank>();
                var combo = playJson["maxcombo"].Value<int>();
                var count300 = playJson["count300"].Value<int>();
                var count100 = playJson["count100"].Value<int>();
                var count50 = playJson["count50"].Value<int>();
                var countMiss = playJson["countmiss"].Value<int>();
                var pp = playJson["pp"].Value<double>();

                var play = new Play(playerId, beatmapId, mods, rank, combo, count300, count100, count50, countMiss, pp);
                result.Add(play);
            }

            return result;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
            _requestRateSemaphore.Dispose();
        }
    }
}