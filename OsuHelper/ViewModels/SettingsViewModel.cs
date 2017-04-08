using System;
using OsuHelper.Services;

namespace OsuHelper.ViewModels
{
    public class SettingsViewModel : ISettingsViewModel
    {
        public ISettingsService SettingsService { get; }

        public SettingsViewModel(ISettingsService settingsService)
        {
            if (settingsService == null)
                throw new ArgumentNullException(nameof(settingsService));

            SettingsService = settingsService;
        }
    }
}