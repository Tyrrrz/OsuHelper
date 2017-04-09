using System.Collections.Generic;
using System.Text.RegularExpressions;
using OsuHelper.Models;
using Tyrrrz.Extensions;
using Tyrrrz.Settings;

namespace OsuHelper.Services
{
    public class FileSettingsService : SettingsManager, ISettingsService
    {
        private string _userId;

        public string UserId
        {
            get { return _userId; }
            set
            {
                string fromUrl = Regex.Match(value, @".*?.ppy.sh/\w/([\w\d]+)").Groups[1].Value;
                _userId = fromUrl.IsNotBlank() ? fromUrl : value;
            }
        }

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