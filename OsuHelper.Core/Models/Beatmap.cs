using System;

namespace OsuHelper.Models
{
    public class Beatmap
    {
        public string Id { get; set; }

        public string MapSetId { get; set; }

        public GameMode GameMode { get; set; }

        public BeatmapRankingStatus RankingStatus { get; set; }

        public string Creator { get; set; }

        public DateTime LastUpdate { get; set; }

        public string Artist { get; set; }

        public string Title { get; set; }

        public string DifficultyName { get; set; }

        public string FullName => $"{Artist} - {Title} [{DifficultyName}]";

        public TimeSpan Length { get; set; }

        public int MaxCombo { get; set; }

        public double BeatsPerMinute { get; set; }

        public double Stars { get; set; }

        public double CircleSize { get; set; }

        public double OverallDifficulty { get; set; }

        public double ApproachRate { get; set; }

        public double Drain { get; set; }

        public string ThumbnailUrl => $"https://b.ppy.sh/thumb/{MapSetId}l.jpg";

        public string CoverUrl => $"https://assets.ppy.sh/beatmaps/{MapSetId}/covers/cover.jpg";

        public string CardUrl => $"https://assets.ppy.sh/beatmaps/{MapSetId}/covers/card.jpg";

        public string SoundPreviewUrl => $"https://b.ppy.sh/preview/{MapSetId}.mp3";
    }
}