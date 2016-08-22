// ------------------------------------------------------------------ 
//  Solution: <OsuHelper>
//  Project: <OsuHelper>
//  File: <RecommenderViewModel.cs>
//  Created By: Alexey Golub
//  Date: 20/08/2016
// ------------------------------------------------------------------ 

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Data;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using NegativeLayer.Extensions;
using OsuHelper.Models.API;
using OsuHelper.Models.Internal;
using OsuHelper.Services;

namespace OsuHelper.ViewModels
{
    public sealed class RecommenderViewModel : ViewModelBase
    {
        private readonly APIService _apiService;
        private readonly WindowService _windowService;
        private readonly ICollectionView _collectionView;

        private BeatmapRecommendation _selectedRecommendation;
        private bool? _hrFilter;
        private bool? _dtFilter;
        private bool? _hdFilter;
        private bool _canUpdate = true;
        private double _progress;

        public ObservableCollection<BeatmapRecommendation> Recommendations { get; } =
            new ObservableCollection<BeatmapRecommendation>();

        public BeatmapRecommendation SelectedRecommendation
        {
            get { return _selectedRecommendation; }
            set
            {
                Set(ref _selectedRecommendation, value);
                
                // Hack (mvvm breaking, view models not decoupled)
                if (value?.Beatmap?.ID != null && value.Beatmap.Mode == GameMode.Standard)
                {
                    Locator.Resolve<CalculatorViewModel>().BeatmapID = value.Beatmap.ID;
                    Locator.Resolve<CalculatorViewModel>().ExpectedAccuracy = value.ExpectedAccuracy;
                    Locator.Resolve<CalculatorViewModel>().Mods = value.Mods;
                }
            }
        }

        public bool? HrFilter
        {
            get { return _hrFilter; }
            set
            {
                Set(ref _hrFilter, value);
                UpdateFilter();
            }
        }

        public bool? DtFilter
        {
            get { return _dtFilter; }
            set
            {
                Set(ref _dtFilter, value);
                UpdateFilter();
            }
        }

        public bool? HdFilter
        {
            get { return _hdFilter; }
            set
            {
                Set(ref _hdFilter, value);
                UpdateFilter();
            }
        }

        public bool CanUpdate
        {
            get { return _canUpdate; }
            set
            {
                Set(ref _canUpdate, value);
                RaisePropertyChanged(() => IsBusy);
                UpdateCommand.RaiseCanExecuteChanged();
            }
        }

        public bool IsBusy => !CanUpdate;

        public double Progress
        {
            get { return _progress; }
            set { Set(ref _progress, value); }
        } 

        // Commands
        public RelayCommand UpdateCommand { get; }
        public RelayCommand<Beatmap> OpenBeatmapPageCommand { get; }
        public RelayCommand<Beatmap> DirectDownloadBeatmapCommand { get; }
        public RelayCommand<Beatmap> DownloadBeatmapCommand { get; }
        public RelayCommand<Beatmap> BloodcatDownloadBeatmapCommand { get; }

        public RecommenderViewModel(APIService apiService, WindowService windowService)
        {
            _apiService = apiService;
            _windowService = windowService;
            _collectionView = CollectionViewSource.GetDefaultView(Recommendations);

            // Default sort
            _collectionView.SortDescriptions.Add(new SortDescription(nameof(BeatmapRecommendation.Popularity),
                ListSortDirection.Descending));

            // Load last recommendations
            SetRecommendations(Persistence.Default.LastRecommendations);

            // Commands
            UpdateCommand = new RelayCommand(Update, () => CanUpdate);
            OpenBeatmapPageCommand = new RelayCommand<Beatmap>(OpenBeatmapPage);
            DirectDownloadBeatmapCommand = new RelayCommand<Beatmap>(DirectDownloadBeatmap);
            DownloadBeatmapCommand = new RelayCommand<Beatmap>(DownloadBeatmap);
            BloodcatDownloadBeatmapCommand = new RelayCommand<Beatmap>(BloodcatDownloadBeatmap);
        }

        private void OpenBeatmapPage(Beatmap bm)
        {
            Process.Start($"https://osu.ppy.sh/b/{bm.ID}");
        }

        private void DirectDownloadBeatmap(Beatmap bm)
        {
            Process.Start($"osu://dl/{bm.MapSetID}");
        }

        private void DownloadBeatmap(Beatmap bm)
        {
            Process.Start(Settings.Default.PreferNoVideo
                ? $"https://osu.ppy.sh/d/{bm.MapSetID}n"
                : $"https://osu.ppy.sh/d/{bm.MapSetID}");
        }

        private void BloodcatDownloadBeatmap(Beatmap bm)
        {
            Process.Start($"http://bloodcat.com/osu/s/{bm.MapSetID}");
        }

        private void SetRecommendations(IEnumerable<BeatmapRecommendation> recommendations)
        {
            var array = recommendations.ToArray();
            Recommendations.Clear();
            array.ForEach(Recommendations.Add);
            Persistence.Default.LastRecommendations = array;
        }

        private void UpdateFilter()
        {
            _collectionView.Filter = o =>
            {
                var rec = (BeatmapRecommendation) o;
                bool hrCheck = !HrFilter.HasValue || HrFilter.Value == rec.Mods.HasFlag(EnabledMods.HardRock);
                bool dtCheck = !DtFilter.HasValue || DtFilter.Value == rec.Mods.HasFlag(EnabledMods.DoubleTime);
                bool hdCheck = !HdFilter.HasValue || HdFilter.Value == rec.Mods.HasFlag(EnabledMods.Hidden);
                return hrCheck && dtCheck && hdCheck;
            };
        }

        private async void Update()
        {
            // TODO: refactor this shit

            CanUpdate = false;
            Progress = 0;
            Debug.WriteLine("Update started", "Beatmap Recommender");

            // Copy settings
            var apiProvider = Settings.Default.APIProvider;
            string apiKey = Settings.Default.APIKey;
            string userID = Settings.Default.UserID;
            var gameMode = Settings.Default.GameMode;
            bool onlyFullCombo = Settings.Default.OnlyFullCombo;
            int ownPlayCountToScan = Settings.Default.OwnPlayCountToScan;
            int othersPlayCountToScan = Settings.Default.OthersPlayCountToScan;
            int similarPlayCount = Settings.Default.SimilarPlayCount;
            int recommendationCount = Settings.Default.RecommendationCount;

            // Check user id
            if (userID.IsBlank())
            {
                _windowService.ShowError("UserID is not valid!");
                CanUpdate = true;
                return;
            }

            // Set up api config
            var config = new APIServiceConfiguration(apiProvider, apiKey);

            // Check API key
            bool apiKeyValid = await _apiService.TestConfigurationAsync(config);
            if (!apiKeyValid)
            {
                _windowService.ShowError("API key is not valid! Pressing OK will open a page where you can get one.");
                Process.Start("https://osu.ppy.sh/p/api");
                CanUpdate = true;
                return;
            }

            // Prepare result
            var recommendations = new List<BeatmapRecommendation>();
            var recommendationsTemp = new List<Play>();

            // Get user's top plays
            var userTopPlays = (await _apiService.GetUserTopPlaysAsync(config, gameMode, userID))
                .OrderByDescending(p => p.PerformancePoints)
                .ToArray();

            // Check if there are any plays
            if (!userTopPlays.Any())
            {
                _windowService.ShowError(
                    $"Could not obtain any top plays set by the user (ID: {userID})!" +
                    Environment.NewLine +
                    "Either the user id is incorrect, the user has no ranked plays or the user is restricted/banned.");
                CanUpdate = true;
                return;
            }
            Debug.WriteLine($"Obtained (Count:{userTopPlays.Length}) user's top plays", "Beatmap Recommender");

            // Ignore beatmaps with plays on them
            var ignoredBeatmaps = userTopPlays.Select(p => p.BeatmapID);
            Debug.WriteLine("Composed a list of ignored beatmaps", "Beatmap Recommender");

            // Leave only top XX plays
            userTopPlays = userTopPlays.Take(ownPlayCountToScan).ToArray();

            // Get boundaries
            double minPP = Math.Floor(userTopPlays.Min(p => p.PerformancePoints)); // 100% of user's lowest top play
            double maxPP = Math.Ceiling(userTopPlays.Max(p => p.PerformancePoints))*1.2; // 120% of user's highest top play

            // Loop through first XX top plays
            foreach (var userPlay in userTopPlays.AsParallel())
            {
                // Get the map's top plays
                var mapTopPlays = (await _apiService.GetBeatmapTopPlaysAsync(config, gameMode, userPlay.BeatmapID, userPlay.Mods)).ToArray();
                if (!mapTopPlays.Any())
                    continue;
                Debug.WriteLine($"Obtained top plays for map (ID:{userPlay.BeatmapID}, Count:{mapTopPlays.Length})", "Beatmap Recommender");

                // Order by PP difference and take YY most similar plays
                var similarTopPlays = mapTopPlays
                    .OrderBy(p => Math.Abs(p.PerformancePoints - userPlay.PerformancePoints))
                    .Take(similarPlayCount)
                    .ToArray();

                // Go through each play's user
                foreach (string similarUserID in similarTopPlays
                    .Select(p => p.UserID)
                    .Distinct()
                    .AsParallel())
                {
                    // Get their top plays
                    Play[] similarUserTopPlays;
                    try
                    {
                        similarUserTopPlays = (await _apiService.GetUserTopPlaysAsync(config, gameMode, similarUserID)).ToArray();
                    }
                    catch
                    {
                        Debug.WriteLine(
                        $"Failed to obtain top plays for similar user (ID:{similarUserID}) based on map (ID:{userPlay.BeatmapID})",
                        "Beatmap Recommender");
                        continue;
                    }
                    if (!similarUserTopPlays.Any())
                        continue;
                    Debug.WriteLine(
                        $"Obtained top plays for similar user (ID:{similarUserID}) based on map (ID:{userPlay.BeatmapID})",
                        "Beatmap Recommender");

                    // Order by PP difference and take ZZ most similar plays
                    var potentialRecommendations = similarUserTopPlays
                        .OrderBy(p => Math.Abs(p.PerformancePoints - userPlay.PerformancePoints))
                        .Where(p => p.PerformancePoints >= minPP && p.PerformancePoints <= maxPP)
                        .Where(p => !ignoredBeatmaps.Contains(p.BeatmapID))
                        .Where(p => !onlyFullCombo || p.Rank >= PlayRank.S)
                        .Take(othersPlayCountToScan);

                    // Add to list
                    recommendationsTemp.AddRange(potentialRecommendations);
                }

                Progress += 0.25*(1.0/userTopPlays.Length);
            }
            Debug.WriteLine("Finished scanning for potential recommendations", "Beatmap Recommender");

            // Group recommendations by beatmap
            var recommendationGroups = recommendationsTemp.GroupBy(p => p.BeatmapID).ToArray();

            // Take only as many as we need
            if (recommendationGroups.Any())
                recommendationGroups = recommendationGroups.Take(recommendationCount).ToArray();

            // Loop through recommendation groups
            foreach (var recommendationGroup in recommendationGroups.AsParallel())
            {
                int count = recommendationGroup.Count();

                Debug.WriteLine(
                    $"Analyzing recommendation group for beatmap (ID:{recommendationGroup.Key}) with {count} items in it",
                    "Beatmap Recommender");

                // Get the median recommendation (based on PP gain). If it needs to decide between the two, it picks second.
                Play median;
                if (count == 1)
                {
                    median = recommendationGroup.First();
                }
                else
                {
                    var ordered = recommendationGroup.OrderBy(p => p.PerformancePoints);
                    int middleIndex = count/2;
                    median = ordered.ElementAt(middleIndex);
                }

                // Get the beatmap data
                Beatmap beatmap;
                try
                {
                    beatmap = await _apiService.GetBeatmapAsync(config, gameMode, recommendationGroup.Key);
                }
                catch
                {
                    Debug.WriteLine($"Failed to obtain beatmap data (ID:{recommendationGroup.Key})", "Beatmap Recommender");
                    continue;
                }
                Debug.WriteLine($"Obtained beatmap data (ID:{recommendationGroup.Key})", "Beatmap Recommender");

                // Add the recommendation
                recommendations.Add(new BeatmapRecommendation(
                    beatmap,
                    median.PerformancePoints,
                    median.Accuracy,
                    median.Mods,
                    count));

                Progress += 0.75*(1.0/recommendationGroups.Length);
            }

            // Sort the recommendations by PP and push it to the property value
            Debug.WriteLine($"Obtained {recommendations.Count} recommendations", "Beatmap Recommender");
            SetRecommendations(recommendations);

            Debug.WriteLine("Done", "Beatmap Recommender");

            Progress = 1;
            CanUpdate = true;
        }
    }
}