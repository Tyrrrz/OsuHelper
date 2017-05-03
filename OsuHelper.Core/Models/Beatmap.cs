using System;

namespace OsuHelper.Models
{
    public class Beatmap
    {
        public string Id { get; set; }

        public string MapSetId { get; set; }

        public GameMode GameMode { get; set; }

        public string Creator { get; set; }

        public DateTime LastUpdate { get; set; }

        public string Artist { get; set; }

        public string Title { get; set; }

        public string Version { get; set; }

        public BeatmapTraits Traits { get; set; }

        public string FullName => $"{Artist} - {Title} [{Version}]";

        public string ThumbnailUrl => $"https://b.ppy.sh/thumb/{MapSetId}l.jpg";

        public string CoverUrl => $"https://assets.ppy.sh/beatmaps/{MapSetId}/covers/cover.jpg";

        public string CardUrl => $"https://assets.ppy.sh/beatmaps/{MapSetId}/covers/card.jpg";
    }
}