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
        public Beatmap Beatmap { get; }

        public double ExpectedPerformancePoints { get; }

        public double RequiredAccuracy { get; }

        public BeatmapRecommendation(Beatmap beatmap, double expectedPerformancePoints, double requiredAccuracy)
        {
            Beatmap = beatmap;
            ExpectedPerformancePoints = expectedPerformancePoints;
            RequiredAccuracy = requiredAccuracy;
        }
    }
}