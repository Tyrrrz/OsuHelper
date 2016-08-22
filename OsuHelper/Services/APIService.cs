// ------------------------------------------------------------------ 
//  Solution: <OsuHelper>
//  Project: <OsuHelper>
//  File: <APIService.cs>
//  Created By: Alexey Golub
//  Date: 20/08/2016
// ------------------------------------------------------------------ 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OsuHelper.Models.API;
using OsuHelper.Models.Internal;

namespace OsuHelper.Services
{
    public sealed class APIService : IDisposable
    {
        private static string URLEncode(string arg)
        {
            return Uri.EscapeUriString(arg);
        }

        private static string GetAPIHome(APIProvider apiProvider)
        {
            if (apiProvider == APIProvider.Osu)
                return "https://osu.ppy.sh/api/";
            if (apiProvider == APIProvider.Ripple)
                return "https://ripple.moe/api/v1/";
            return null;
        }

        private readonly WebClient _webClient = new WebClient();

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
                await _webClient.DownloadStringTaskAsync(url);
            }
            catch (WebException ex)
            {
                if (((HttpWebResponse) ex.Response).StatusCode == HttpStatusCode.Unauthorized)
                    return false;
            }
            return true;
        }

        public async Task<Beatmap> GetBeatmapAsync(APIServiceConfiguration config, string id)
        {
            string home = GetAPIHome(config.APIProvider);
            string url = home + $"get_beatmaps?k={config.APIKey}&b={id}&limit=1";
            string response = await _webClient.DownloadStringTaskAsync(url);
            return JsonConvert.DeserializeObject<Beatmap[]>(response).FirstOrDefault();
        }

        public async Task<IEnumerable<Play>> GetUserTopPlaysAsync(APIServiceConfiguration config, string userID)
        {
            string home = GetAPIHome(config.APIProvider);
            string url = home + $"get_user_best?k={config.APIKey}&u={URLEncode(userID)}&limit=100";
            string response = await _webClient.DownloadStringTaskAsync(url);
            return JsonConvert.DeserializeObject<Play[]>(response);
        }

        public async Task<IEnumerable<Play>> GetBeatmapTopPlaysAsync(APIServiceConfiguration config, string mapID, EnabledMods mods = EnabledMods.Any)
        {
            string home = GetAPIHome(config.APIProvider);
            string url = home + $"get_scores?k={config.APIKey}&b={mapID}&limit=100";
            if (mods != EnabledMods.Any)
                url += $"&mods={(int) mods}";
            string response = await _webClient.DownloadStringTaskAsync(url);
            return JsonConvert.DeserializeObject<Play[]>(response);
        }

        public void Dispose()
        {
            _webClient.Dispose();
        }
    }
}