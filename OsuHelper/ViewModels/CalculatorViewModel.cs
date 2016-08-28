// ------------------------------------------------------------------ 
//  Solution: <OsuHelper>
//  Project: <OsuHelper>
//  File: <CalculatorViewModel.cs>
//  Created By: Alexey Golub
//  Date: 22/08/2016
// ------------------------------------------------------------------ 

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using NegativeLayer.Extensions;
using OsuHelper.Models.API;
using OsuHelper.Services;

namespace OsuHelper.ViewModels
{
    public sealed class CalculatorViewModel : ViewModelBase, IDisposable
    {
        private readonly OppaiService _oppaiService;
        private readonly WindowService _windowService;
        private readonly OsuGameService _osuGameService;
        private readonly OsuSearchService _osuSearchService;
        private readonly WebClient _webClient;

        private string _beatmapFilePath;
        private bool _updateQueued;

        private string _beatmapID;
        private bool _isAnalyzed;
        private bool _canAnalyze = true;
        private bool _canUpdate;
        private EnabledMods _mods = EnabledMods.None;
        private double _expectedAccuracy = 0.95;
        private double _expectedPerformancePoints;

        public string BeatmapID
        {
            get { return _beatmapID; }
            set
            {
                if (Set(ref _beatmapID, value))
                {
                    _beatmapFilePath = null;
                    IsAnalyzed = false;
                    CanUpdate = false;
                }
            }
        }

        public bool IsAnalyzed
        {
            get { return _isAnalyzed; }
            private set { Set(ref _isAnalyzed, value); }
        }

        public bool CanAnalyze
        {
            get { return _canAnalyze; }
            private set
            {
                Set(ref _canAnalyze, value);
                AnalyzeCommand.RaiseCanExecuteChanged();
            }
        }

        public bool CanUpdate
        {
            get { return _canUpdate; }
            private set
            {
                if (value && _updateQueued)
                {
                    _updateQueued = false;
                    Update();
                    return;
                }
                Set(ref _canUpdate, value);
            }
        }

        public EnabledMods Mods
        {
            get { return _mods; }
            set
            {
                Set(ref _mods, value);
                RaisePropertyChanged(() => HrEnabled);
                RaisePropertyChanged(() => DtEnabled);
                RaisePropertyChanged(() => HdEnabled);
                TryUpdate();
            }
        }

        public bool HrEnabled
        {
            get { return Mods.HasFlag(EnabledMods.HardRock); }
            set
            {
                if (value)
                    Mods |= EnabledMods.HardRock;
                else
                    Mods &= ~EnabledMods.HardRock;
            }
        }

        public bool DtEnabled
        {
            get { return Mods.HasFlag(EnabledMods.DoubleTime); }
            set
            {
                if (value)
                    Mods |= EnabledMods.DoubleTime;
                else
                    Mods &= ~EnabledMods.DoubleTime;
            }
        }

        public bool HdEnabled
        {
            get { return Mods.HasFlag(EnabledMods.Hidden); }
            set
            {
                if (value)
                    Mods |= EnabledMods.Hidden;
                else
                    Mods &= ~EnabledMods.Hidden;
            }
        }

        public double ExpectedAccuracy
        {
            get { return _expectedAccuracy; }
            set
            {
                Set(ref _expectedAccuracy, value);
                RaisePropertyChanged(() => ExpectedAccuracyString);
                TryUpdate();
            }
        }

        public string ExpectedAccuracyString
        {
            get { return (ExpectedAccuracy*100.0).ToString("n2"); }
            set { ExpectedAccuracy = (value.Without("%").ParseDoubleOrDefault()/100).Clamp(0, 1); }
        }

        public double ExpectedPerformancePoints
        {
            get { return _expectedPerformancePoints; }
            set { Set(ref _expectedPerformancePoints, value); }
        }

        // Commands
        public RelayCommand SetCurrentlyPlayingBeatmapIDCommand { get; }
        public RelayCommand AnalyzeCommand { get; }

        public CalculatorViewModel(OppaiService oppaiService, WindowService windowService, OsuGameService osuGameService,
            OsuSearchService osuSearchService)
        {
            _oppaiService = oppaiService;
            _windowService = windowService;
            _osuGameService = osuGameService;
            _osuSearchService = osuSearchService;
            _webClient = new WebClient();

            // Commands
            AnalyzeCommand = new RelayCommand(Analyze, () => CanAnalyze);
            SetCurrentlyPlayingBeatmapIDCommand = new RelayCommand(SetCurrentlyPlayingBeatmapID);
        }

        private async void SetCurrentlyPlayingBeatmapID()
        {
            string fullTitle = _osuGameService.GetNowPlayingTitle();
            if (fullTitle.IsBlank())
            {
                _windowService.ShowError("Currently not playing any beatmap");
                return;
            }
            var match = Regex.Match(fullTitle, @"(.*?)\s-\s(.*?)\[(.*?)\]");
            if (!match.Success)
            {
                _windowService.ShowError($"Unable to parse beatmap title ({fullTitle})");
                return;
            }

            // Get results
            string artist = match.Groups[1].Value;
            string title = match.Groups[2].Value;
            string difficulty = match.Groups[3].Value;

            // Search (only on standard)
            var beatmapIDs = (await _osuSearchService.SearchAsync(GameMode.Standard, artist, title, difficulty)).ToArray();
            if (!beatmapIDs.Any())
            {
                _windowService.ShowError($"Could not resolve a ranked beatmap by title ({fullTitle})");
                return;
            }

            // Get first ID and copy it over
            BeatmapID = beatmapIDs.First();
        }

        private async Task DownloadMap()
        {
            // Parse out the ID if needed
            var match = Regex.Match(BeatmapID, @".*?osu.ppy.sh/\w/(\d+)");
            if (match.Success)
                BeatmapID = match.Groups[1].Value;

            string downloadUrl = $"https://osu.ppy.sh/osu/{BeatmapID}";
            _beatmapFilePath = FileSystem.GetTempFile("osu_helper_map", "osu");
            await _webClient.DownloadFileTaskAsync(downloadUrl, _beatmapFilePath);
        }

        private async void Analyze()
        {
            CanAnalyze = false;

            // Check id
            if (BeatmapID.IsBlank())
            {
                _windowService.ShowError("Beatmap ID can't be empty!");
                CanAnalyze = true;
                return;
            }

            // Download beatmap
            try
            {
                await DownloadMap();
            }
            catch
            {
                _windowService.ShowError("Could not download the map!");
                CanAnalyze = true;
                return;
            }

            // Run oppai on it once
            Update();

            IsAnalyzed = true;
            CanUpdate = true;
            CanAnalyze = true;
        }

        private async void Update()
        {
            CanUpdate = false;

            // Make sure the map file is still there
            if (_beatmapFilePath.IsBlank() || !File.Exists(_beatmapFilePath))
                return;

            ExpectedPerformancePoints = await _oppaiService.CalculatePerformancePointsAsync(_beatmapFilePath, ExpectedAccuracy, Mods);

            CanUpdate = true;
        }

        private void TryUpdate()
        {
            if (CanUpdate)
                Update();
            else
                _updateQueued = true;
        }

        public void Dispose()
        {
            _webClient.Dispose();
        }
    }
}