// ------------------------------------------------------------------ 
//  Solution: <OsuHelper>
//  Project: <OsuHelper>
//  File: <APIService.cs>
//  Created By: Alexey Golub
//  Date: 20/08/2016
// ------------------------------------------------------------------ 

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using NegativeLayer.Extensions;
using Newtonsoft.Json;
using OsuHelper.Models.API;
using OsuHelper.Models.Internal;

namespace OsuHelper.Services
{
    public sealed class APIService
    {
        private static string GetAPIHome(APIProvider apiProvider)
        {
            if (apiProvider == APIProvider.Osu)
                return "https://osu.ppy.sh/api/";
            if (apiProvider == APIProvider.Ripple)
                return "https://ripple.moe/api/";
            return null;
        }

        private static WebClient GetWebClient()
        {
            return new WebClient();
        }

        private static async Task<string> GetStringAsync(string url)
        {
            using (var client = GetWebClient())
                return await client.DownloadStringTaskAsync(url);
        }

        /// <summary>
        /// Tests the given api configuration
        /// </summary>
        /// <returns>True if everything is working, false otherwise</returns>
        public async Task<bool> TestConfigurationAsync(APIServiceConfiguration config)
        {
            // Ripple doesn't have API keys
            if (config.APIProvider == APIProvider.Ripple)
                return true;

            // Query a random API endpoint with the given key and see what happens
            string home = GetAPIHome(config.APIProvider);
            string url = home + $"get_beatmaps?k={config.APIKey}&b=1&limit=1";
            try
            {
                await GetStringAsync(url);
            }
            catch (WebException ex)
            {
                if (((HttpWebResponse) ex.Response).StatusCode == HttpStatusCode.Unauthorized)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Get beatmap by its beatmap ID for the given game mode
        /// </summary>
        public async Task<Beatmap> GetBeatmapAsync(APIServiceConfiguration config, GameMode mode, string id)
        {
            string home = GetAPIHome(config.APIProvider);
            string url = home + $"get_beatmaps?k={config.APIKey}&m={(int) mode}&b={id}&limit=1&a=1";
            string response = await GetStringAsync(url);
            return JsonConvert.DeserializeObject<Beatmap[]>(response).FirstOrDefault();
        }

        /// <summary>
        /// Get top plays of a user by their user ID or user name
        /// </summary>
        public async Task<IEnumerable<Play>> GetUserTopPlaysAsync(APIServiceConfiguration config, GameMode mode, string userID)
        {
            string home = GetAPIHome(config.APIProvider);
            string url = home + $"get_user_best?k={config.APIKey}&m={(int) mode}&u={userID.UrlEncode()}&limit=100";
            string response = await GetStringAsync(url);
            return JsonConvert.DeserializeObject<Play[]>(response);
        }

        /// <summary>
        /// Get top plays of a beatmap by its beatmap ID, for the given game mode, using given mods
        /// </summary>
        public async Task<IEnumerable<Play>> GetBeatmapTopPlaysAsync(APIServiceConfiguration config, GameMode mode,
            string mapID, EnabledMods mods = EnabledMods.Any)
        {
            string home = GetAPIHome(config.APIProvider);
            string url = home + $"get_scores?k={config.APIKey}&m={(int) mode}&b={mapID}&limit=100";
            if (mods != EnabledMods.Any)
                url += $"&mods={(int) mods}";
            string response = await GetStringAsync(url);
            return JsonConvert.DeserializeObject<Play[]>(response);
        }
    }
}