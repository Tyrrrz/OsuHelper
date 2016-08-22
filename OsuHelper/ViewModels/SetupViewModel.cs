// ------------------------------------------------------------------ 
//  Solution: <OsuHelper>
//  Project: <OsuHelper>
//  File: <SetupViewModel.cs>
//  Created By: Alexey Golub
//  Date: 20/08/2016
// ------------------------------------------------------------------ 

using System;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using OsuHelper.Models.API;

namespace OsuHelper.ViewModels
{
    public class SetupViewModel : ViewModelBase
    {
        private bool _isAPIKeyRequired;

        public Settings Settings => Settings.Default;
        public APIProvider[] AvailableAPIProviders => Enum.GetValues(typeof (APIProvider)).Cast<APIProvider>().ToArray();

        public bool IsAPIKeyRequired
        {
            get { return _isAPIKeyRequired; }
            set { Set(ref _isAPIKeyRequired, value); }
        }

        // Commands
        public RelayCommand SaveCommand { get; }
        public RelayCommand LoadCommand { get; }
        public RelayCommand ResetDefaultsCommand { get; }

        public SetupViewModel()
        {
            IsAPIKeyRequired = Settings.APIProvider == APIProvider.Osu;

            // Commands
            SaveCommand = new RelayCommand(Save, () => !Settings.IsSaved);
            LoadCommand = new RelayCommand(Load, () => !Settings.IsSaved);
            ResetDefaultsCommand = new RelayCommand(ResetDefaults);

            // Events
            Settings.PropertyChanged += (sender, args) =>
            {
                IsAPIKeyRequired = Settings.APIProvider == APIProvider.Osu;
            };
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

            SaveCommand.RaiseCanExecuteChanged();
            LoadCommand.RaiseCanExecuteChanged();
        }
    }
}