using GalaSoft.MvvmLight.Command;
using OsuHelper.Models;

namespace OsuHelper.ViewModels
{
    public interface ISettingsViewModel
    {
        string UserId { get; set; }

        string ApiKey { get; set; }

        GameMode GameMode { get; set; }

        bool DownloadWithoutVideo { get; set; }

        RelayCommand GetApiKeyCommand { get; }
    }
}