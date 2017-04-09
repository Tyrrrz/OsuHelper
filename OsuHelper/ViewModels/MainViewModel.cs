using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using OsuHelper.Models;
using OsuHelper.Services;

namespace OsuHelper.ViewModels
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        private readonly ISettingsService _settingsService;
        private readonly IRecommendationService _recommendationService;

        private bool _isBusy;
        private IEnumerable<BeatmapRecommendation> _recommendations;

        public bool IsBusy
        {
            get { return _isBusy; }
            private set { Set(ref _isBusy, value); }
        }

        public IEnumerable<BeatmapRecommendation> Recommendations
        {
            get { return _recommendations; }
            private set { Set(ref _recommendations, value); }
        }

        public RelayCommand GetRecommendationsCommand { get; }

        public MainViewModel(ISettingsService settingsService, IRecommendationService recommendationService)
        {
            if (settingsService == null)
                throw new ArgumentNullException(nameof(settingsService));
            if (recommendationService == null)
                throw new ArgumentNullException(nameof(recommendationService));

            _settingsService = settingsService;
            _recommendationService = recommendationService;

            // Commands
            GetRecommendationsCommand = new RelayCommand(GetRecommendationsAsync);

            // Load stored recommendations
            Recommendations = _settingsService.LastRecommendations;
        }

        private async void GetRecommendationsAsync()
        {
            IsBusy = true;

            Recommendations = await _recommendationService.GetRecommendationsAsync();
            _settingsService.LastRecommendations = Recommendations;

            IsBusy = false;
        }
    }
}