using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
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
        private readonly ICacheService _cacheService;
        private readonly IRecommendationService _recommendationService;

        private bool _isBusy;
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

        public RelayCommand ShowAboutCommand { get; }
        public RelayCommand ShowSettingsCommand { get; }
        public RelayCommand ShowBeatmapDetailsCommand { get; }
        public RelayCommand PopulateRecommendationsCommand { get; }

        public MainViewModel(ISettingsService settingsService, ICacheService cacheService,
            IRecommendationService recommendationService)
        {
            _settingsService = settingsService;
            _cacheService = cacheService;
            _recommendationService = recommendationService;

            // Commands
            ShowAboutCommand = new RelayCommand(ShowAbout);
            ShowSettingsCommand = new RelayCommand(ShowSettings);
            ShowBeatmapDetailsCommand = new RelayCommand(ShowBeatmapDetails);
            PopulateRecommendationsCommand = new RelayCommand(PopulateRecommendations, () => !IsBusy);

            // Load last recommendations
            _recommendations =
                _cacheService.RetrieveOrDefault<IReadOnlyList<Recommendation>>("LastRecommendations");
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
                MessengerInstance.Send(new ShowNotificationMessage("Not configured",
                    "User ID and/or API key are not set. Please specify them in settings."));
                return;
            }

            IsBusy = true;

            try
            {
                Recommendations = await _recommendationService.GetRecommendationsAsync();
                _cacheService.Store("LastRecommendations", Recommendations);
            }
            catch (RecommendationsUnavailableException ex)
            {
                MessengerInstance.Send(new ShowNotificationMessage("Recommendations unavailable", ex.Reason));
            }
            catch (HttpErrorStatusCodeException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                MessengerInstance.Send(new ShowNotificationMessage("Unauthorized",
                    "Please make sure your API key is valid."));
            }

            IsBusy = false;
        }
    }
}