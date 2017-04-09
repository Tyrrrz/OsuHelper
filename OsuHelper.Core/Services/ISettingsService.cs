using System.Collections.Generic;
using OsuHelper.Models;

namespace OsuHelper.Services
{
    public interface ISettingsService
    {
        string UserId { get; set; }
        string ApiRoot { get; set; }
        string ApiKey { get; set; }
        GameMode GameMode { get; set; }
        bool ShouldDownloadWithoutVideo { get; set; }

        IEnumerable<BeatmapRecommendation> LastRecommendations { get; set; }

        void Load();
        void Save();
    }
}