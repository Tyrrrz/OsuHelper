using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using OsuHelper.Exceptions;
using OsuHelper.Messages;
using OsuHelper.Models;
using OsuHelper.Services;
using Tyrrrz.Extensions;

namespace OsuHelper.ViewModels
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        private readonly ISettingsService _settingsService;
        private readonly IUpdateService _updateService;
        private readonly ICacheService _cacheService;
        private readonly IRecommendationService _recommendationService;

        private bool _isBusy;
        private double _progress;
        private IReadOnlyList<Recommendation> _recommendations;
        private Recommendation _selectedRecommendation;

        public bool IsBusy
        {
            get => _isBusy;
            private set
            {
                Set(ref _isBusy, value);
                PopulateRecommendationsCommand.RaiseCanExecuteChanged();
            }
        }

        public bool HasData => Recommendations.NotNullAndAny();

        public double Progress
        {
            get => _progress;
            private set => Set(ref _progress, value);
        }

        public IReadOnlyList<Recommendation> Recommendations
        {
            get => _recommendations;
            private set
            {
                Set(ref _recommendations, value);
                RaisePropertyChanged(() => HasData);
            }
        }

        public Recommendation SelectedRecommendation
        {
            get => _selectedRecommendation;
            set => Set(ref _selectedRecommendation, value);
        }

        public RelayCommand ViewLoadedCommand { get; }
        public RelayCommand ViewClosedCommand { get; }
        public RelayCommand ShowAboutCommand { get; }
        public RelayCommand ShowSettingsCommand { get; }
        public RelayCommand ShowBeatmapDetailsCommand { get; }
        public RelayCommand PopulateRecommendationsCommand { get; }

        public MainViewModel(ISettingsService settingsService, IUpdateService updateService,
            ICacheService cacheService, IRecommendationService recommendationService)
        {
            _settingsService = settingsService;
            _updateService = updateService;
            _cacheService = cacheService;
            _recommendationService = recommendationService;

            // Commands
            ViewLoadedCommand = new RelayCommand(ViewLoaded);
            ViewClosedCommand = new RelayCommand(ViewClosed);
            ShowAboutCommand = new RelayCommand(ShowAbout);
            ShowSettingsCommand = new RelayCommand(ShowSettings);
            ShowBeatmapDetailsCommand = new RelayCommand(ShowBeatmapDetails);
            PopulateRecommendationsCommand = new RelayCommand(PopulateRecommendations, () => !IsBusy);
        }

        private async void ViewLoaded()
        {
            // Load settings
            _settingsService.Load();

            // Load last recommendations
            Recommendations = _cacheService.RetrieveOrDefault<IReadOnlyList<Recommendation>>("LastRecommendations");

            // Check and prepare update
            try
            {
                var updateVersion = await _updateService.CheckPrepareUpdateAsync();
                if (updateVersion != null)
                {
                    MessengerInstance.Send(new ShowNotificationMessage(
                        $"Update to osu!helper v{updateVersion} will be installed when you exit",
                        "INSTALL NOW", () =>
                        {
                            _updateService.NeedRestart = true;
                            Application.Current.Shutdown();
                        }));
                }
            }
            catch
            {
                MessengerInstance.Send(new ShowNotificationMessage("Failed to perform application auto-update"));
            }
        }

        private async void ViewClosed()
        {
            // Save settings
            _settingsService.Save();

            // Finalize updates if available
            await _updateService.FinalizeUpdateAsync();
        }

        private void ShowSettings()
        {
            MessengerInstance.Send(new ShowSettingsMessage());
        }

        private void ShowBeatmapDetails()
        {
            var beatmap = SelectedRecommendation?.Beatmap;
            if (beatmap == null) return;

            MessengerInstance.Send(new ShowBeatmapDetailsMessage(beatmap));
        }

        private void ShowAbout()
        {
            Process.Start("https://github.com/Tyrrrz/OsuHelper");
        }

        private async void PopulateRecommendations()
        {
            // Validate settings
            if (_settingsService.UserId.IsBlank() || _settingsService.ApiKey.IsBlank())
            {
                MessengerInstance.Send(new ShowNotificationMessage(
                    "Not configured – set username and API key in settings",
                    "OPEN", ShowSettings));
                return;
            }

            IsBusy = true;
            Progress = 0;

            try
            {
                var progressHandler = new Progress<double>(p => Progress = p);
                Recommendations = await _recommendationService.GetRecommendationsAsync(progressHandler);
                _cacheService.Store("LastRecommendations", Recommendations);
            }
            catch (TopPlaysUnavailableException)
            {
                MessengerInstance.Send(new ShowNotificationMessage("Recommendations unavailable – no top plays set in selected game mode"));
            }
            catch (HttpErrorStatusCodeException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                MessengerInstance.Send(new ShowNotificationMessage("Unauthorized – make sure API key is valid"));
            }

            IsBusy = false;
            Progress = 0;
        }
    }
}