using System;

namespace OsuHelper.Models
{
    public class BeatmapTraits
    {
        public int MaxCombo { get; }

        public TimeSpan Duration { get; }

        public double BeatsPerMinute { get; }

        public double StarRating { get; }

        public double ApproachRate { get; }

        public double OverallDifficulty { get; }

        public double CircleSize { get; }

        public double Drain { get; }

        public BeatmapTraits(int maxCombo, TimeSpan duration, double beatsPerMinute, double starRating,
            double approachRate, double overallDifficulty, double circleSize, double drain)
        {
            MaxCombo = maxCombo;
            Duration = duration;
            BeatsPerMinute = beatsPerMinute;
            StarRating = starRating;
            ApproachRate = approachRate;
            OverallDifficulty = overallDifficulty;
            CircleSize = circleSize;
            Drain = drain;
        }
    }
}