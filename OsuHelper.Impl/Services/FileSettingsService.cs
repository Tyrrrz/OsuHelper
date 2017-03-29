using System.Collections.Generic;
using OsuHelper.Models;
using Tyrrrz.Settings;

namespace OsuHelper.Services
{
    public class FileSettingsService : SettingsManager, ISettingsService
    {
        public IEnumerable<BeatmapRecommendation> LastRecommendations { get; set; }
        public string OsuApiRoot { get; set; } = "https://osu.ppy.sh/api/";
        public string OsuApiKey { get; set; }

        public FileSettingsService()
        {
            Configuration.FileName = "Configuration.dat";
            Configuration.SubDirectoryPath = "";
            Configuration.StorageSpace = StorageSpace.Instance;
        }
    }
}