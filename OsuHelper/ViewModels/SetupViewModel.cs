// ------------------------------------------------------------------ 
//  Solution: <OsuHelper>
//  Project: <OsuHelper>
//  File: <SetupViewModel.cs>
//  Created By: Alexey Golub
//  Date: 20/08/2016
// ------------------------------------------------------------------ 

using System;
using System.Linq;
using System.Text.RegularExpressions;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using OsuHelper.Models.API;
using OsuHelper.Models.Internal;

namespace OsuHelper.ViewModels
{
    public sealed class SetupViewModel : ViewModelBase
    {
        private bool _isAPIKeyRequired;

        public Settings Settings => Settings.Default;
        public APIProvider[] AvailableAPIProviders => Enum.GetValues(typeof (APIProvider)).Cast<APIProvider>().ToArray();
        public GameMode[] AvailableGameModes => Enum.GetValues(typeof (GameMode)).Cast<GameMode>().ToArray();

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
                if (args.PropertyName == nameof(Settings.APIProvider))
                    IsAPIKeyRequired = Settings.APIProvider == APIProvider.Osu;
            };
        }

        private void Save()
        {
            // Convert user profile links to user IDs if necessary
            var match = Regex.Match(Settings.UserID, @".*?osu.ppy.sh/\w/([\w\d]+)");
            if (match.Success)
                Settings.UserID = match.Groups[1].Value;

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