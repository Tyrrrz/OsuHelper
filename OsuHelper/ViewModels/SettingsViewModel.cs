using System.Diagnostics;
using System.Text.RegularExpressions;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using OsuHelper.Models;
using OsuHelper.Services;
using Tyrrrz.Extensions;

namespace OsuHelper.ViewModels
{
    public class SettingsViewModel : ViewModelBase, ISettingsViewModel
    {
        private readonly ISettingsService _settingsService;

        public string UserId
        {
            get => _settingsService.UserId;
            set
            {
                if (value != null)
                {
                    var fromUrl = Regex.Match(value, @".*?.ppy.sh/\w/([\w\d]+)").Groups[1].Value;
                    _settingsService.UserId = fromUrl.IsNotBlank() ? fromUrl : value;
                }
                else
                {
                    _settingsService.UserId = null;
                }
            }
        }

        public string ApiKey
        {
            get => _settingsService.ApiKey;
            set => _settingsService.ApiKey = value;
        }

        public GameMode GameMode
        {
            get => _settingsService.GameMode;
            set => _settingsService.GameMode = value;
        }

        public bool DownloadWithoutVideo
        {
            get => _settingsService.DownloadWithoutVideo;
            set => _settingsService.DownloadWithoutVideo = value;
        }

        public RelayCommand GetApiKeyCommand { get; }

        public SettingsViewModel(ISettingsService settingsService)
        {
            _settingsService = settingsService;

            // Commands
            GetApiKeyCommand = new RelayCommand(GetApiKey);
        }

        private void GetApiKey()
        {
            Process.Start("https://osu.ppy.sh/p/api");
        }
    }
}