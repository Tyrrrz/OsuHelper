using OsuHelper.Models;

namespace OsuHelper.Services
{
    public interface ISettingsService
    {
        string UserId { get; set; }
        string ApiKey { get; set; }
        GameMode GameMode { get; set; }
        bool DownloadWithoutVideo { get; set; }
        double PreviewVolume { get; set; }
        bool IsAutoUpdateEnabled { get; set; }

        void Load();
        void Save();
    }
}