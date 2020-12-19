using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using OsuHelper.Internal;
using OsuHelper.Models;
using OsuHelper.Services;
using OsuHelper.ViewModels.Framework;
using Tyrrrz.Extensions;

namespace OsuHelper.ViewModels.Dialogs
{
    public class SettingsViewModel : DialogScreen
    {
        private readonly SettingsService _settingsService;

        public string? UserId
        {
            get => _settingsService.UserId;
            set
            {
                if (value != null)
                {
                    var fromUrl = Regex.Match(value, @".*?.ppy.sh/\w/([\w\d]+)").Groups[1].Value;
                    _settingsService.UserId = !string.IsNullOrWhiteSpace(fromUrl) ? fromUrl : value;
                }
                else
                {
                    _settingsService.UserId = null;
                }
            }
        }

        public string? ApiKey
        {
            get => _settingsService.ApiKey;
            set => _settingsService.ApiKey = value;
        }

        public IReadOnlyList<GameMode> AvailableGameModes { get; } =
            Enum.GetValues(typeof(GameMode)).Cast<GameMode>().ToArray();

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

        public double PreviewVolume
        {
            get => _settingsService.PreviewVolume;
            set => _settingsService.PreviewVolume = value.Clamp(0, 1);
        }

        public bool IsAutoUpdateEnabled
        {
            get => _settingsService.IsAutoUpdateEnabled;
            set => _settingsService.IsAutoUpdateEnabled = value;
        }

        public SettingsViewModel(SettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public void ObtainApiKey() => ProcessEx.StartShellExecute("https://osu.ppy.sh/p/api/");
    }
}