using System;

namespace OsuHelper.Models
{
    public class BeatmapRecommendation
    {
        public int Popularity { get; set; }

        public Beatmap Beatmap { get; set; }

        public Mods Mods { get; set; }

        public BeatmapTraits TraitsWithMods { get; set; }

        public bool IsDurationAffected
            => Math.Abs((Beatmap.Traits.Duration - TraitsWithMods.Duration).TotalSeconds) > 0.01;

        public bool IsBeatsPerMinuteAffected
            => Math.Abs(Beatmap.Traits.BeatsPerMinute - TraitsWithMods.BeatsPerMinute) > 0.01;

        public bool IsStarRatingAffected
            => Math.Abs(Beatmap.Traits.StarRating - TraitsWithMods.StarRating) > 0.01;

        public bool IsApproachRateAffected
            => Math.Abs(Beatmap.Traits.ApproachRate - TraitsWithMods.ApproachRate) > 0.01;

        public bool IsOverallDifficultyAffected
            => Math.Abs(Beatmap.Traits.OverallDifficulty - TraitsWithMods.OverallDifficulty) > 0.01;

        public bool IsCircleSizeAffected
            => Math.Abs(Beatmap.Traits.CircleSize - TraitsWithMods.CircleSize) > 0.01;

        public bool IsDrainAffected
            => Math.Abs(Beatmap.Traits.Drain - TraitsWithMods.Drain) > 0.01;

        public double ExpectedAccuracy { get; set; }

        public double ExpectedPerformancePoints { get; set; }
    }
}