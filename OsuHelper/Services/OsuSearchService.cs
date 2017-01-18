// -------------------------------------------------------------------------
// Solution: OsuHelper
// Project: OsuHelper
// File: OsuSearchService.cs
// 
// Created by: Tyrrrz
// On: 28.08.2016
// -------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using NegativeLayer.Extensions;
using Newtonsoft.Json;
using OsuHelper.Models.API;

namespace OsuHelper.Services
{
    public sealed class OsuSearchService
    {
        private const string Home = "http://osusearch.com/";

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
        /// Searches for beatmaps using given properties and then returns their beatmap IDs
        /// </summary>
        public async Task<IEnumerable<string>> SearchAsync(GameMode mode = GameMode.Standard, string artist = null, string title = null, string diffName = null)
        {
            string url = Home + "query";
            var args = new List<string>();

            // Hidden parameters
            args.Add("statuses=Ranked");

            // Mandatory parameters
            if (mode == GameMode.Standard)
                args.Add("modes=Standard");
            else if (mode == GameMode.CatchTheBeat)
                args.Add("modes=CtB");
            else if (mode == GameMode.Taiko)
                args.Add("modes=Taiko");
            else if (mode == GameMode.Mania)
                args.Add("modes=Mania");

            // Optional parameters
            if (artist.IsNotBlank())
                args.Add($"artist={artist.Trim().UrlEncode()}");
            if (title.IsNotBlank())
                args.Add($"title={title.Trim().UrlEncode()}");
            if (diffName.IsNotBlank())
                args.Add($"diff_name={diffName.Trim().UrlEncode()}");
            url += "?" + args.JoinToString("&");
            string response = await GetStringAsync(url);

            // High-danger zone (no typechecks)
            dynamic result = JsonConvert.DeserializeObject(response);
            return ((IEnumerable<dynamic>) result.beatmaps).Select(b => b.beatmap_id.ToString()).Cast<string>();
        }
    }
}