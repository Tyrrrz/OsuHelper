using System;

namespace OsuHelper.Models
{
    public class BeatmapTraits
    {
        public int MaxCombo { get; set; }

        public TimeSpan Duration { get; set; }

        public double BeatsPerMinute { get; set; }

        public double StarRating { get; set; }

        public double ApproachRate { get; set; }

        public double OverallDifficulty { get; set; }

        public double CircleSize { get; set; }

        public double Drain { get; set; }
    }
}