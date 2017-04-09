using System.Collections.Generic;
using OsuHelper.Models;
using Tyrrrz.Settings;

namespace OsuHelper.Services
{
    public class FileSettingsService : SettingsManager, ISettingsService
    {
        public string UserId { get; set; }
        public string ApiRoot { get; set; } = "https://osu.ppy.sh/api/";
        public string ApiKey { get; set; }
        public GameMode GameMode { get; set; } = GameMode.Standard;
        public bool ShouldDownloadWithoutVideo { get; set; }
        public IEnumerable<BeatmapRecommendation> LastRecommendations { get; set; }

        public FileSettingsService()
        {
            Configuration.FileName = "Config.dat";
            Configuration.SubDirectoryPath = "";
            Configuration.StorageSpace = StorageSpace.Instance;
        }
    }
}