using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using OsuHelper.Models;
using OsuHelper.Services;
using Tyrrrz.Extensions;

namespace OsuHelper.ViewModels
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        private readonly ISettingsService _settingsService;
        private readonly IRecommendationService _recommendationService;

        private bool _isBusy;
        private IReadOnlyList<BeatmapRecommendation> _recommendations;

        public bool IsBusy
        {
            get => _isBusy;
            private set
            {
                Set(ref _isBusy, value);
                GetRecommendationsCommand.RaiseCanExecuteChanged();
            }
        }

        public IReadOnlyList<BeatmapRecommendation> Recommendations
        {
            get => _recommendations;
            private set
            {
                Set(ref _recommendations, value);
                RaisePropertyChanged(() => HasData);
            }
        }

        public bool HasData => Recommendations.NotNullAndAny();

        public RelayCommand GetRecommendationsCommand { get; }

        public MainViewModel(ISettingsService settingsService, IRecommendationService recommendationService)
        {
            _settingsService = settingsService;
            _recommendationService = recommendationService;

            // Commands
            GetRecommendationsCommand = new RelayCommand(GetRecommendationsAsync, () => !IsBusy);

            // Load stored recommendations
            _recommendations = _settingsService.LastRecommendations;
        }

        private async void GetRecommendationsAsync()
        {
            IsBusy = true;

            Recommendations = (await _recommendationService.GetRecommendationsAsync()).ToArray();
            _settingsService.LastRecommendations = Recommendations;

            IsBusy = false;
        }
    }
}