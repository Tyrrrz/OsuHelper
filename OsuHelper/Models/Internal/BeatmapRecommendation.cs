// ------------------------------------------------------------------ 
//  Solution: <OsuHelper>
//  Project: <OsuHelper>
//  File: <BeatmapRecommendation.cs>
//  Created By: Alexey Golub
//  Date: 20/08/2016
// ------------------------------------------------------------------ 

using OsuHelper.Models.API;

namespace OsuHelper.Models.Internal
{
    public class BeatmapRecommendation
    {
        public Beatmap Beatmap { get; set; }

        public double ExpectedPerformancePoints { get; set; }

        public double RequiredAccuracy { get; set; }
    }
}