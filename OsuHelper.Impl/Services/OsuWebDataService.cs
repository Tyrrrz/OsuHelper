using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using OsuHelper.Models;
using Tyrrrz.Extensions;

namespace OsuHelper.Services
{
    public class OsuWebDataService : IDataService
    {
        private readonly ISettingsService _settingsService;
        private readonly IHttpService _httpService;
        private readonly ICacheService _cacheService;

        private string OsuWebRoot => _settingsService.OsuWebRoot.Trim('/');
        private string OsuArtifactsRoot => _settingsService.OsuArtifactsRoot.Trim('/');
        private string OsuApiKey => _settingsService.OsuApiKey;

        public OsuWebDataService(ISettingsService settingsService, IHttpService httpService, ICacheService cacheService)
        {
            _settingsService = settingsService;
            _httpService = httpService;
            _cacheService = cacheService;
        }

        public async Task<Beatmap> GetBeatmapAsync(string beatmapId, GameMode gameMode)
        {
            // Try to get from cache first
            var cached = _cacheService.RetrieveOrDefault<Beatmap>(beatmapId);
            if (cached != null) return cached;

            // Get
            var url = OsuWebRoot + $"/api/get_beatmaps?k={OsuApiKey}&m={(int) gameMode}&b={beatmapId}&limit=1&a=1";
            var response = await _httpService.GetStringAsync(url);

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
            _cacheService.Store(beatmapId, result);

            return result;
        }

        public async Task<string> GetBeatmapRawAsync(string beatmapId)
        {
            // Try get from cache first
            var cached = _cacheService.RetrieveOrDefault<string>($"BeatmapRaw_{beatmapId}");
            if (cached != null) return cached;

            // Get
            var url = OsuWebRoot + $"/osu/{beatmapId}";
            var response = await _httpService.GetStringAsync(url);

            // Save to cache
            _cacheService.Store($"BeatmapRaw_{beatmapId}", response);

            return response;
        }

        public async Task<Stream> GetMapSetPreviewAsync(string mapSetId)
        {
            // Try get from cache first
            var cached = _cacheService.RetrieveOrDefault<Stream>($"BeatmapPreview_{mapSetId}");
            if (cached != null) return cached;

            // Get
            var url = OsuArtifactsRoot + $"/preview/{mapSetId}.mp3";
            var response = await _httpService.GetStreamAsync(url);

            // Save to cache
            _cacheService.Store($"BeatmapPreview_{mapSetId}", response);

            return _cacheService.RetrieveOrDefault<Stream>($"BeatmapPreview_{mapSetId}");
        }

        public async Task<IEnumerable<Play>> GetUserTopPlaysAsync(string userId, GameMode gameMode)
        {
            // Don't cache volatile data

            // Get
            var url = OsuWebRoot +
                         $"/api/get_user_best?k={OsuApiKey}&m={(int) gameMode}&u={userId.UrlEncode()}&limit=100";
            var response = await _httpService.GetStringAsync(url);

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

        public async Task<IEnumerable<Play>> GetBeatmapTopPlaysAsync(string beatmapId, GameMode gameMode, Mods mods)
        {
            // Don't cache volatile data

            // Get
            var url = OsuWebRoot + $"/api/get_scores?k={OsuApiKey}&m={(int) gameMode}&b={beatmapId}&limit=100";
            var response = await _httpService.GetStringAsync(url);

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
    }
}