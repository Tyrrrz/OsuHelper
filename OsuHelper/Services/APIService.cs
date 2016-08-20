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
using Newtonsoft.Json;
using OsuHelper.Models.API;

namespace OsuHelper.Services
{
    public class APIService
    {
        private const string APIHome = "https://osu.ppy.sh/api/";
        private static string APIKey => Settings.Default.APIKey;

        private readonly WebClient _client = new WebClient();

        public async Task<Beatmap> GetBeatmapAsync(string id)
        {
            string url = APIHome + $"get_beatmaps?k={APIKey}&b={id}&limit=1";
            string response = await _client.DownloadStringTaskAsync(url);
            return JsonConvert.DeserializeObject<Beatmap[]>(response).FirstOrDefault();
        }

        public async Task<IEnumerable<Play>> GetUserTopPlays(string userID)
        {
            string url = APIHome + $"get_user_best?k={APIKey}&u={userID}&limit=100";
            string response = await _client.DownloadStringTaskAsync(url);
            return JsonConvert.DeserializeObject<Play[]>(response);
        }

        public async Task<IEnumerable<Play>> GetBeatmapTopPlays(string mapID, EnabledMods mods = EnabledMods.Any)
        {
            string url = APIHome + $"get_scores?k={APIKey}&b={mapID}&limit=100";
            if (mods != EnabledMods.Any)
                url += $"&mods={(int) mods}";
            string response = await _client.DownloadStringTaskAsync(url);
            return JsonConvert.DeserializeObject<Play[]>(response);
        }
    }
}