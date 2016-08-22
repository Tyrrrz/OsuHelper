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

namespace OsuHelper.Services
{
    public sealed class APIService : IDisposable
    {
        private static string URLEncode(string arg)
        {
            return Uri.EscapeUriString(arg);
        }

        private readonly WebClient _client = new WebClient();
        private APIProvider _apiProvider;
        private string _apiKey;

        private string APIHome
        {
            get
            {
                if (_apiProvider == APIProvider.Osu)
                    return "https://osu.ppy.sh/api/";
                if (_apiProvider == APIProvider.Ripple)
                    return "https://ripple.moe/api/v1/";
                return null;
            }
        }

        public void SetupAPI(APIProvider apiProvider, string key)
        {
            _apiProvider = apiProvider;
            _apiKey = key;
        }

        public async Task<bool> TestAPIKey()
        {
            // Ripple doesn't have API keys
            if (_apiProvider == APIProvider.Ripple)
                return true;

            // Query a random API endpoint with the given key and see what happens
            string url = APIHome + $"get_beatmaps?k={_apiKey}&b=1&limit=1";
            try
            {
                await _client.DownloadStringTaskAsync(url);
            }
            catch (WebException ex)
            {
                if (((HttpWebResponse) ex.Response).StatusCode == HttpStatusCode.Unauthorized)
                    return false;
            }
            return true;
        }

        public async Task<Beatmap> GetBeatmapAsync(string id)
        {
            string url = APIHome + $"get_beatmaps?k={_apiKey}&b={id}&limit=1";
            string response = await _client.DownloadStringTaskAsync(url);
            return JsonConvert.DeserializeObject<Beatmap[]>(response).FirstOrDefault();
        }

        public async Task<IEnumerable<Play>> GetUserTopPlaysAsync(string userID)
        {
            string url = APIHome + $"get_user_best?k={_apiKey}&u={URLEncode(userID)}&limit=100";
            string response = await _client.DownloadStringTaskAsync(url);
            return JsonConvert.DeserializeObject<Play[]>(response);
        }

        public async Task<IEnumerable<Play>> GetBeatmapTopPlaysAsync(string mapID, EnabledMods mods = EnabledMods.Any)
        {
            string url = APIHome + $"get_scores?k={_apiKey}&b={mapID}&limit=100";
            if (mods != EnabledMods.Any)
                url += $"&mods={(int) mods}";
            string response = await _client.DownloadStringTaskAsync(url);
            return JsonConvert.DeserializeObject<Play[]>(response);
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}