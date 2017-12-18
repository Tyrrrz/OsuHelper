using System;

namespace OsuHelper.Models
{
    public class BeatmapRecommendation
    {
        public int Popularity { get; }

        public Beatmap Beatmap { get; }

        public Mods Mods { get; }

        public BeatmapTraits TraitsWithMods { get; }

        public bool IsDurationAffected
            => Math.Abs((Beatmap.Traits.Duration - TraitsWithMods.Duration).TotalSeconds) >= 0.01;

        public bool IsBeatsPerMinuteAffected
            => Math.Abs(Beatmap.Traits.BeatsPerMinute - TraitsWithMods.BeatsPerMinute) >= 0.01;

        public bool IsStarRatingAffected
            => Math.Abs(Beatmap.Traits.StarRating - TraitsWithMods.StarRating) >= 0.01;

        public bool IsApproachRateAffected
            => Math.Abs(Beatmap.Traits.ApproachRate - TraitsWithMods.ApproachRate) >= 0.01;

        public bool IsOverallDifficultyAffected
            => Math.Abs(Beatmap.Traits.OverallDifficulty - TraitsWithMods.OverallDifficulty) >= 0.01;

        public bool IsCircleSizeAffected
            => Math.Abs(Beatmap.Traits.CircleSize - TraitsWithMods.CircleSize) >= 0.01;

        public bool IsDrainAffected
            => Math.Abs(Beatmap.Traits.Drain - TraitsWithMods.Drain) >= 0.01;

        public double ExpectedAccuracy { get; }

        public double ExpectedPerformancePoints { get; }

        public BeatmapRecommendation(int popularity, Beatmap beatmap, Mods mods, BeatmapTraits traitsWithMods,
            double expectedAccuracy, double expectedPerformancePoints)
        {
            Popularity = popularity;
            Beatmap = beatmap;
            Mods = mods;
            TraitsWithMods = traitsWithMods;
            ExpectedAccuracy = expectedAccuracy;
            ExpectedPerformancePoints = expectedPerformancePoints;
        }
    }
}