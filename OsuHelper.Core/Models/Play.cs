namespace OsuHelper.Models
{
    public class Play
    {
        public string PlayerId { get; set; }

        public string BeatmapId { get; set; }

        public Mods Mods { get; set; }

        public PlayRank Rank { get; set; }

        public int MaxCombo { get; set; }

        public int Count300 { get; set; }

        public int Count100 { get; set; }

        public int Count50 { get; set; }

        public int CountMiss { get; set; }

        public double Accuracy
            => (Count50*50 + Count100*100 + Count300*300)/(300.0*(Count50 + Count100 + Count300 + CountMiss));

        public double PerformancePoints { get; set; }
    }
}