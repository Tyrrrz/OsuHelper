using System;

namespace OsuHelper.Models
{
    public class BeatmapRecommendation
    {
        public int Popularity { get; set; }

        public Beatmap Beatmap { get; set; }

        public EnabledMods Mods { get; set; }

        public double ExpectedAccuracy { get; set; }

        public double ExpectedPerformancePoints { get; set; }

        public TimeSpan ActualDuration => Beatmap.Duration;

        public double ActualBeatsPerMinute => Beatmap.BeatsPerMinute;

        public double ActualStars => Beatmap.Stars;

        public double ActualApproachRate => Beatmap.ApproachRate;

        public double ActualOverallDifficulty => Beatmap.OverallDifficulty;

        public double ActualCircleSize => Beatmap.CircleSize;

        public double ActualDrain => Beatmap.Drain;
    }
}