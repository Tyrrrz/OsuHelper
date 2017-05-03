using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using OsuHelper.Models;
using Tyrrrz.Extensions;

namespace OsuHelper.Services
{
    public class OsuApiDataService : IDataService
    {
        private readonly ISettingsService _settingsService;
        private readonly IHttpService _httpService;

        private string ApiRoot => _settingsService.ApiRoot.Trim('/');
        private string ApiKey => _settingsService.ApiKey;

        public OsuApiDataService(ISettingsService settingsService, IHttpService httpService)
        {
            _settingsService = settingsService;
            _httpService = httpService;
        }

        public async Task<Beatmap> GetBeatmapAsync(string beatmapId, GameMode gameMode)
        {
            // Get
            string url = ApiRoot + $"/get_beatmaps?k={ApiKey}&m={(int) gameMode}&b={beatmapId}&limit=1&a=1";
            string response = await _httpService.GetStringAsync(url);

            // Parse
            var parsed = JToken.Parse(response).First;

            // Extract data
            var result = new Beatmap();
            result.Id = parsed["beatmap_id"].Value<string>();
            result.MapSetId = parsed["beatmapset_id"].Value<string>();
            result.GameMode = gameMode;
            result.Creator = parsed["creator"].Value<string>();
            result.LastUpdate = parsed["last_update"].Value<DateTime>();
            result.Artist = parsed["artist"].Value<string>();
            result.Title = parsed["title"].Value<string>();
            result.Version = parsed["version"].Value<string>();
            result.Traits = new BeatmapTraits();
            result.Traits.MaxCombo = parsed["max_combo"].Value<int?>().GetValueOrDefault(); // can be null sometimes
            result.Traits.Duration = TimeSpan.FromSeconds(parsed["hit_length"].Value<double>());
            result.Traits.BeatsPerMinute = parsed["bpm"].Value<double>();
            result.Traits.StarRating = parsed["difficultyrating"].Value<double>();
            result.Traits.ApproachRate = parsed["diff_approach"].Value<double>();
            result.Traits.OverallDifficulty = parsed["diff_overall"].Value<double>();
            result.Traits.CircleSize = parsed["diff_size"].Value<double>();
            result.Traits.Drain = parsed["diff_drain"].Value<double>();

            return result;
        }

        public async Task<IEnumerable<Play>> GetUserTopPlaysAsync(string userId, GameMode gameMode)
        {
            // Get
            string url = ApiRoot + $"/get_user_best?k={ApiKey}&m={(int) gameMode}&u={userId.UrlEncode()}&limit=100";
            string response = await _httpService.GetStringAsync(url);

            // Parse
            var parsed = JToken.Parse(response);

            // Extract data
            var result = new List<Play>();
            foreach (var jPlay in parsed)
            {
                var play = new Play();
                play.PlayerId = jPlay["user_id"].Value<string>();
                play.BeatmapId = jPlay["beatmap_id"].Value<string>();
                play.Mods = (Mods) jPlay["enabled_mods"].Value<int>();
                play.Rank = jPlay["rank"].Value<string>().ParseEnum<PlayRank>();
                play.MaxCombo = jPlay["maxcombo"].Value<int>();
                play.Count300 = jPlay["count300"].Value<int>();
                play.Count100 = jPlay["count100"].Value<int>();
                play.Count50 = jPlay["count50"].Value<int>();
                play.CountMiss = jPlay["countmiss"].Value<int>();
                play.PerformancePoints = jPlay["pp"].Value<double>();

                result.Add(play);
            }

            return result;
        }

        public async Task<IEnumerable<Play>> GetBeatmapTopPlaysAsync(string beatmapId, GameMode gameMode, Mods mods)
        {
            // Get
            string url = ApiRoot + $"/get_scores?k={ApiKey}&m={(int) gameMode}&b={beatmapId}&limit=100";
            string response = await _httpService.GetStringAsync(url);

            // Parse
            var parsed = JToken.Parse(response);

            // Extract data
            var result = new List<Play>();
            foreach (var jPlay in parsed)
            {
                var play = new Play();
                play.PlayerId = jPlay["user_id"].Value<string>();
                play.BeatmapId = beatmapId;
                play.Mods = (Mods) jPlay["enabled_mods"].Value<int>();
                play.Rank = jPlay["rank"].Value<string>().ParseEnum<PlayRank>();
                play.MaxCombo = jPlay["maxcombo"].Value<int>();
                play.Count300 = jPlay["count300"].Value<int>();
                play.Count100 = jPlay["count100"].Value<int>();
                play.Count50 = jPlay["count50"].Value<int>();
                play.CountMiss = jPlay["countmiss"].Value<int>();
                play.PerformancePoints = jPlay["pp"].Value<double>();

                result.Add(play);
            }

            return result;
        }
    }
}