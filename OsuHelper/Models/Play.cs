namespace OsuHelper.Models
{
    public class Play
    {
        public string PlayerId { get; }

        public string BeatmapId { get; }

        public Mods Mods { get; }

        public PlayRank Rank { get; }

        public int Combo { get; }

        public int Count300 { get; }

        public int Count100 { get; }

        public int Count50 { get; }

        public int CountMiss { get; }

        public double Accuracy =>
            (50 * Count50 + 100 * Count100 + 300 * Count300) /
            (300.0 * (Count50 + Count100 + Count300 + CountMiss));

        public double PerformancePoints { get; }

        public Play(string playerId, string beatmapId, Mods mods, PlayRank rank, int combo, int count300,
            int count100, int count50, int countMiss, double performancePoints)
        {
            PlayerId = playerId;
            BeatmapId = beatmapId;
            Mods = mods;
            Rank = rank;
            Combo = combo;
            Count300 = count300;
            Count100 = count100;
            Count50 = count50;
            CountMiss = countMiss;
            PerformancePoints = performancePoints;
        }
    }
}