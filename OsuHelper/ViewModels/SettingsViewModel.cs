using System.Diagnostics;
using GalaSoft.MvvmLight.Command;
using OsuHelper.Services;

namespace OsuHelper.ViewModels
{
    public class SettingsViewModel : ISettingsViewModel
    {
        private string OsuWebRoot => SettingsService.OsuWebRoot.Trim('/');

        public ISettingsService SettingsService { get; }

        public RelayCommand GetApiKeyCommand { get; }

        public SettingsViewModel(ISettingsService settingsService)
        {
            SettingsService = settingsService;

            // Commands
            GetApiKeyCommand = new RelayCommand(GetApiKey);
        }

        private void GetApiKey()
        {
            Process.Start(OsuWebRoot + "/p/api");
        }
    }
}