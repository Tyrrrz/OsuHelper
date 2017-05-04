using GalaSoft.MvvmLight.Command;
using OsuHelper.Services;

namespace OsuHelper.ViewModels
{
    public interface ISettingsViewModel
    {
        ISettingsService SettingsService { get; }

        RelayCommand GetApiKeyCommand { get; }
    }
}