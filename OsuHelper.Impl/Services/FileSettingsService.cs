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
            get => _userId;
            set
            {
                if (value != null)
                {
                    string fromUrl = Regex.Match(value, @".*?.ppy.sh/\w/([\w\d]+)").Groups[1].Value;
                    _userId = fromUrl.IsNotBlank() ? fromUrl : value;
                }
                else
                {
                    _userId = null;
                }
            }
        }

        public string OsuWebRoot { get; set; } = "https://osu.ppy.sh/";

        public string OsuApiKey { get; set; }

        public GameMode GameMode { get; set; } = GameMode.Standard;

        public bool DownloadWithoutVideo { get; set; }

        public IReadOnlyList<BeatmapRecommendation> LastRecommendations { get; set; }

        public FileSettingsService()
        {
            Configuration.FileName = "Config.dat";
            Configuration.SubDirectoryPath = "";
            Configuration.StorageSpace = StorageSpace.Instance;
        }
    }
}