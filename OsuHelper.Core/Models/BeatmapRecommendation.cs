namespace OsuHelper.Models
{
    public class BeatmapRecommendation
    {
        public int Popularity { get; set; }

        public Beatmap Beatmap { get; set; }

        public EnabledMods Mods { get; set; }

        public double ExpectedAccuracy { get; set; }

        public double ExpectedPerformancePoints { get; set; }

        public double ActualBeatsPerMinute { get; set; }

        public double ActualStars { get; set; }

        public double ActualCircleSize { get; set; }

        public double ActualOverallDifficulty { get; set; }

        public double ActualApproachRate { get; set; }

        public double ActualDrain { get; set; }
    }
}