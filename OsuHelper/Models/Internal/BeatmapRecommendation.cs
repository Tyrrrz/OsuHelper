// ------------------------------------------------------------------ 
//  Solution: <OsuHelper>
//  Project: <OsuHelper>
//  File: <BeatmapRecommendation.cs>
//  Created By: Alexey Golub
//  Date: 20/08/2016
// ------------------------------------------------------------------ 

using Newtonsoft.Json;
using OsuHelper.Models.API;

namespace OsuHelper.Models.Internal
{
    public class BeatmapRecommendation
    {
        public Beatmap Beatmap { get; }

        public double ExpectedPerformancePoints { get; }

        public double ExpectedAccuracy { get; }

        public EnabledMods Mods { get; }

        public double Popularity { get; }

        [JsonIgnore]
        public string ModsString => Mods.GetModsString();

        public BeatmapRecommendation(
            Beatmap beatmap,
            double expectedPerformancePoints,
            double expectedAccuracy,
            EnabledMods mods,
            double popularity)
        {
            Beatmap = beatmap;
            ExpectedPerformancePoints = expectedPerformancePoints;
            ExpectedAccuracy = expectedAccuracy;
            Mods = mods;
            Popularity = popularity;
        }
    }
}