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
        private const string APIHome = "https://osu.ppy.sh/api/";

        private readonly WebClient _client = new WebClient();

        public async Task<bool> TestAPIKey(string key)
        {
            string url = APIHome + $"get_beatmaps?k={key}&b=1&limit=1";
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

        public async Task<Beatmap> GetBeatmapAsync(string key, string id)
        {
            string url = APIHome + $"get_beatmaps?k={key}&b={id}&limit=1";
            string response = await _client.DownloadStringTaskAsync(url);
            return JsonConvert.DeserializeObject<Beatmap[]>(response).FirstOrDefault();
        }

        public async Task<IEnumerable<Play>> GetUserTopPlaysAsync(string key, string userID)
        {
            string url = APIHome + $"get_user_best?k={key}&u={userID}&limit=100";
            string response = await _client.DownloadStringTaskAsync(url);
            return JsonConvert.DeserializeObject<Play[]>(response);
        }

        public async Task<IEnumerable<Play>> GetBeatmapTopPlaysAsync(string key, string mapID, EnabledMods mods = EnabledMods.Any)
        {
            string url = APIHome + $"get_scores?k={key}&b={mapID}&limit=100";
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