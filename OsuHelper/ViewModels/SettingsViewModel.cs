using OsuHelper.Services;

namespace OsuHelper.ViewModels
{
    public class SettingsViewModel : ISettingsViewModel
    {
        public ISettingsService SettingsService { get; }

        public SettingsViewModel(ISettingsService settingsService)
        {
            SettingsService = settingsService;
        }
    }
}