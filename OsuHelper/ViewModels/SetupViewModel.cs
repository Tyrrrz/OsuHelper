// ------------------------------------------------------------------ 
//  Solution: <OsuHelper>
//  Project: <OsuHelper>
//  File: <SetupViewModel.cs>
//  Created By: Alexey Golub
//  Date: 20/08/2016
// ------------------------------------------------------------------ 

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace OsuHelper.ViewModels
{
    public class SetupViewModel : ViewModelBase
    {
        public Settings Settings => Settings.Default;

        // Commands
        public RelayCommand SaveCommand { get; }
        public RelayCommand LoadCommand { get; }
        public RelayCommand ResetDefaultsCommand { get; }

        public SetupViewModel()
        {
            // Commands
            SaveCommand = new RelayCommand(Save, () => !Settings.IsSaved);
            LoadCommand = new RelayCommand(Load, () => !Settings.IsSaved);
            ResetDefaultsCommand = new RelayCommand(ResetDefaults);
        }

        private void Save()
        {
            Settings.Save();

            SaveCommand.RaiseCanExecuteChanged();
            LoadCommand.RaiseCanExecuteChanged();
        }

        private void Load()
        {
            Settings.Load();

            SaveCommand.RaiseCanExecuteChanged();
            LoadCommand.RaiseCanExecuteChanged();
        }

        private void ResetDefaults()
        {
            Settings.Reset();
        }
    }
}