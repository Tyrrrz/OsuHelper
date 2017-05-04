using System.Collections.Generic;
using OsuHelper.Models;

namespace OsuHelper.Services
{
    public interface ISettingsService
    {
        string UserId { get; set; }
        string OsuRoot { get; set; }
        string ApiKey { get; set; }
        GameMode GameMode { get; set; }
        bool DownloadWithoutVideo { get; set; }

        IEnumerable<BeatmapRecommendation> LastRecommendations { get; set; }

        void Load();
        void Save();
    }
}