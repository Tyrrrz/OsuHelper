// ------------------------------------------------------------------ 
//  Solution: <OsuHelper>
//  Project: <OsuHelper>
//  File: <RecommenderViewModel.cs>
//  Created By: Alexey Golub
//  Date: 20/08/2016
// ------------------------------------------------------------------ 

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using NegativeLayer.Extensions;
using OsuHelper.Models.API;
using OsuHelper.Models.Internal;
using OsuHelper.Services;

namespace OsuHelper.ViewModels
{
    public class RecommenderViewModel : ViewModelBase
    {
        private readonly APIService _apiService;
        private readonly WindowService _windowService;

        private IEnumerable<BeatmapRecommendation> _recommendations;
        private int _recommendationsCount;
        private bool _canUpdate = true;
        private BeatmapRecommendation _selectedRecommendation;
        private double _progress;

        public IEnumerable<BeatmapRecommendation> Recommendations
        {
            get { return _recommendations; }
            private set
            {
                var array = value?.ToArray() ?? new BeatmapRecommendation[0];
                Set(ref _recommendations, array);
                RecommendationsCount = array.Length;
                Persistence.Default.LastRecommendations = array.ToArray(); // make a copy
            }
        }

        public int RecommendationsCount
        {
            get { return _recommendationsCount; }
            set { Set(ref _recommendationsCount, value); }
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

        public BeatmapRecommendation SelectedRecommendation
        {
            get { return _selectedRecommendation; }
            set { Set(ref _selectedRecommendation, value); }
        }

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

            // Load last recommendations
            Recommendations = Persistence.Default.LastRecommendations;

            // Events
            Settings.Default.PropertyChanged += (sender, args) => UpdateCommand.RaiseCanExecuteChanged();

            // Commands
            UpdateCommand = new RelayCommand(Update,
                () => CanUpdate && Settings.Default.APIKey.IsNotBlank() && Settings.Default.UserID.IsNotBlank());
            OpenBeatmapPageCommand = new RelayCommand<Beatmap>(bm => Process.Start($"https://osu.ppy.sh/b/{bm.ID}"));
            DirectDownloadBeatmapCommand = new RelayCommand<Beatmap>(bm => Process.Start($"osu://dl/{bm.MapSetID}"));
            DownloadBeatmapCommand = new RelayCommand<Beatmap>(bm => Process.Start($"https://osu.ppy.sh/d/{bm.MapSetID}"));
            BloodcatDownloadBeatmapCommand = new RelayCommand<Beatmap>(bm => Process.Start($"http://bloodcat.com/osu/s/{bm.MapSetID}"));
        }

        private async void Update()
        {
            CanUpdate = false;
            Progress = 0;
            Debug.WriteLine("Update started", "Beatmap Recommender");

            // Copy settings
            string apiKey = Settings.Default.APIKey;
            string userID = Settings.Default.UserID;
            int ownPlayCountToScan = Settings.Default.OwnPlayCountToScan;
            int othersPlayCountToScan = Settings.Default.OthersPlayCountToScan;
            int similarPlayCount = Settings.Default.SimilarPlayCount;
            int recommendationCount = Settings.Default.RecommendationCount;

            // Check API key
            bool apiKeyValid = await _apiService.TestAPIKey(apiKey);
            if (!apiKeyValid)
            {
                _windowService.ShowError("API key is not valid!");
                CanUpdate = true;
                return;
            }

            // Prepare result
            var recommendations = new List<BeatmapRecommendation>();
            var recommendationsTemp = new List<Play>();

            // Get user's top plays
            var userTopPlays = (await _apiService.GetUserTopPlaysAsync(apiKey, userID))
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
                var mapTopPlays = (await _apiService.GetBeatmapTopPlaysAsync(apiKey, userPlay.BeatmapID, userPlay.Mods)).ToArray();
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
                    var similarUserTopPlays = (await _apiService.GetUserTopPlaysAsync(apiKey, similarUserID)).ToArray();
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
                    median = recommendationGroup.ElementAt(0);
                }
                else if (count == 2)
                {
                    median = recommendationGroup.ElementAt(1);
                }
                else
                {
                    var ordered = recommendationGroup.OrderBy(p => p.PerformancePoints);
                    int middleIndex = count/2;
                    median = ordered.ElementAt(middleIndex);
                }

                // Get the beatmap data
                var beatmap = await _apiService.GetBeatmapAsync(apiKey, median.BeatmapID);
                Debug.WriteLine($"Obtained beatmap data (ID:{beatmap.ID})", "Beatmap Recommender");

                // Add the recommendation
                recommendations.Add(new BeatmapRecommendation(
                    beatmap,
                    median.PerformancePoints,
                    median.Accuracy,
                    median.Mods));

                Progress += 0.75*(1.0/recommendationGroups.Length);
            }

            // Sort the recommendations by PP and push it to the property value
            Debug.WriteLine($"Obtained {recommendations.Count} recommendations", "Beatmap Recommender");
            Recommendations = recommendations.OrderBy(r => r.ExpectedPerformancePoints);

            Debug.WriteLine("Done", "Beatmap Recommender");
            Progress = 1;
            CanUpdate = true;
        }
    }
}