using System;

namespace OsuHelper.Models
{
    public class BeatmapRecommendation
    {
        public Beatmap Beatmap { get; }

        public int Weight { get; }

        public Mods Mods { get; }

        public BeatmapTraits TraitsWithMods { get; }

        public bool IsDurationAffected
            => Math.Abs((Beatmap.Traits.Duration - TraitsWithMods.Duration).TotalSeconds) >= 0.01;

        public bool IsTempoAffected
            => Math.Abs(Beatmap.Traits.Tempo - TraitsWithMods.Tempo) >= 0.01;

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

        public BeatmapRecommendation(Beatmap beatmap, int weight, Mods mods, BeatmapTraits traitsWithMods,
            double expectedAccuracy, double expectedPerformancePoints)
        {
            Beatmap = beatmap;
            Weight = weight;
            Mods = mods;
            TraitsWithMods = traitsWithMods;
            ExpectedAccuracy = expectedAccuracy;
            ExpectedPerformancePoints = expectedPerformancePoints;
        }
    }
}