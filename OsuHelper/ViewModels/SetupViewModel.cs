// ------------------------------------------------------------------ 
//  Solution: <OsuHelper>
//  Project: <OsuHelper>
//  File: <SetupViewModel.cs>
//  Created By: Alexey Golub
//  Date: 20/08/2016
// ------------------------------------------------------------------ 

using System.Collections.Generic;
using System.Text.RegularExpressions;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using NegativeLayer.Extensions;
using NegativeLayer.Settings;
using OsuHelper.Models.API;
using OsuHelper.Models.Internal;

namespace OsuHelper.ViewModels
{
    public sealed class SetupViewModel : ViewModelBase
    {
        private bool _isAPIKeyRequired;

        public SettingsManagerStager<Settings> Stager => Settings.Stager;
        public Settings StagingSettings => Stager.Staging;
        public IEnumerable<APIProvider> AvailableAPIProviders => typeof (APIProvider).GetAllEnumValues<APIProvider>();
        public IEnumerable<GameMode> AvailableGameModes => typeof (GameMode).GetAllEnumValues<GameMode>();

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
            IsAPIKeyRequired = StagingSettings.APIProvider == APIProvider.Osu;

            // Commands
            SaveCommand = new RelayCommand(Save, () => !StagingSettings.IsSaved);
            LoadCommand = new RelayCommand(Load, () => !StagingSettings.IsSaved);
            ResetDefaultsCommand = new RelayCommand(ResetDefaults);

            // Events
            StagingSettings.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(StagingSettings.APIProvider))
                    IsAPIKeyRequired = StagingSettings.APIProvider == APIProvider.Osu;
            };
        }

        private void Save()
        {
            // Convert user profile links to user IDs if necessary
            var match = Regex.Match(StagingSettings.UserID, @".*?osu.ppy.sh/\w/([\w\d]+)");
            if (match.Success)
                StagingSettings.UserID = match.Groups[1].Value;

            Stager.Save();

            SaveCommand.RaiseCanExecuteChanged();
            LoadCommand.RaiseCanExecuteChanged();
        }

        private void Load()
        {
            Stager.Load();

            SaveCommand.RaiseCanExecuteChanged();
            LoadCommand.RaiseCanExecuteChanged();
        }

        private void ResetDefaults()
        {
            StagingSettings.Reset();

            SaveCommand.RaiseCanExecuteChanged();
            LoadCommand.RaiseCanExecuteChanged();
        }
    }
}