using Tyrrrz.Settings;

namespace OsuHelper.Services
{
    public class FileSettingsService : SettingsManager, ISettingsService
    {
        public string DataSourceApiRoot { get; set; } = "https://osu.ppy.sh/api/";
        public string DataSourceApiKey { get; set; }

        public FileSettingsService()
        {
            Configuration.FileName = "Configuration.dat";
            Configuration.SubDirectoryPath = "";
            Configuration.StorageSpace = StorageSpace.Instance;
        }
    }
}