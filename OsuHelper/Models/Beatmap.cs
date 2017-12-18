using System;

namespace OsuHelper.Models
{
    public class Beatmap
    {
        public string Id { get; }

        public string MapSetId { get; }

        public GameMode GameMode { get; }

        public string Creator { get; }

        public DateTime LastUpdate { get; }

        public string Artist { get; }

        public string Title { get; }

        public string Version { get; }

        public BeatmapTraits Traits { get; }

        public string FullName => $"{Artist} - {Title} [{Version}]";

        public string ThumbnailUrl => $"https://b.ppy.sh/thumb/{MapSetId}l.jpg";

        public string CoverUrl => $"https://assets.ppy.sh/beatmaps/{MapSetId}/covers/cover.jpg";

        public string CardUrl => $"https://assets.ppy.sh/beatmaps/{MapSetId}/covers/card.jpg";

        public Beatmap(string id, string mapSetId, GameMode gameMode, string creator, DateTime lastUpdate,
            string artist, string title, string version, BeatmapTraits traits)
        {
            Id = id;
            MapSetId = mapSetId;
            GameMode = gameMode;
            Creator = creator;
            LastUpdate = lastUpdate;
            Artist = artist;
            Title = title;
            Version = version;
            Traits = traits;
        }
    }
}