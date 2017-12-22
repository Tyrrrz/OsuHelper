using System.Collections.Generic;
using OsuHelper.Models;
using Tyrrrz.Settings;

namespace OsuHelper.Services
{
    public class SettingsService : SettingsManager, ISettingsService
    {
        public string UserId { get; set; }

        public string ApiKey { get; set; }

        public GameMode GameMode { get; set; } = GameMode.Standard;

        public bool DownloadWithoutVideo { get; set; }

        public IReadOnlyList<BeatmapRecommendation> LastRecommendations { get; set; }

        public SettingsService()
        {
            Configuration.FileName = "Config.dat";
            Configuration.SubDirectoryPath = "";
            Configuration.StorageSpace = StorageSpace.Instance;
        }
    }
}