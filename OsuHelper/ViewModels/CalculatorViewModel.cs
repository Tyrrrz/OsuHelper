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
        private readonly WebClient _webClient;

        private string _beatmapFilePath;

        private string _beatmapID;
        private bool _canAnalyze = true;
        private EnabledMods _mods = EnabledMods.None;
        private bool _hrEnabled;
        private bool _dtEnabled;
        private bool _hdEnabled;
        private double _expectedAccuracy = 0.95;
        private double _expectedPerformancePoints;

        public string BeatmapID
        {
            get { return _beatmapID; }
            set { Set(ref _beatmapID, value); }
        }

        public bool CanAnalyze
        {
            get { return _canAnalyze; }
            set
            {
                Set(ref _canAnalyze, value);
                AnalyzeCommand.RaiseCanExecuteChanged();
            }
        }

        public EnabledMods Mods
        {
            get { return _mods; }
            set { Set(ref _mods, value); }
        }

        public bool HrEnabled
        {
            get { return _hrEnabled; }
            set
            {
                Set(ref _hrEnabled, value);
                UpdateEnabledMods();
            }
        }

        public bool DtEnabled
        {
            get { return _dtEnabled; }
            set
            {
                Set(ref _dtEnabled, value);
                UpdateEnabledMods();
            }
        }

        public bool HdEnabled
        {
            get { return _hdEnabled; }
            set
            {
                Set(ref _hdEnabled, value);
                UpdateEnabledMods();
            }
        }

        public double ExpectedAccuracy
        {
            get { return _expectedAccuracy; }
            set
            {
                Set(ref _expectedAccuracy, value);
                RaisePropertyChanged(() => ExpectedAccuracyString);
                Update();
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
        public RelayCommand AnalyzeCommand { get; }

        public CalculatorViewModel(OppaiService oppaiService, WindowService windowService)
        {
            _oppaiService = oppaiService;
            _windowService = windowService;
            _webClient = new WebClient();

            // Commands
            AnalyzeCommand = new RelayCommand(Analyze, () => CanAnalyze);
        }

        private void UpdateEnabledMods()
        {
            Mods = EnabledMods.None;
            if (HrEnabled)
                Mods |= EnabledMods.HardRock;
            if (DtEnabled)
                Mods |= EnabledMods.DoubleTime;
            if (HdEnabled)
                Mods |= EnabledMods.Hidden;
        }

        private async Task DownloadMap()
        {
            // Parse out the ID if needed
            if (!BeatmapID.All(char.IsDigit))
                BeatmapID = Regex.Match(BeatmapID, @".*?osu.ppy.sh/\w/(\d+)").Groups[1].Value;

            string downloadUrl = $"https://osu.ppy.sh/osu/{BeatmapID}";
            _beatmapFilePath = Path.GetTempFileName();
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
            ExpectedPerformancePoints = await _oppaiService.CalculatePerformancePointsAsync(_beatmapFilePath, ExpectedAccuracy, Mods);

            CanAnalyze = true;
        }

        private async void Update()
        {
            // Make sure the map file is still there
            if (_beatmapFilePath.IsBlank() || !File.Exists(_beatmapFilePath))
                await DownloadMap();

            ExpectedPerformancePoints = await _oppaiService.CalculatePerformancePointsAsync(_beatmapFilePath, ExpectedAccuracy, Mods);
        }

        public void Dispose()
        {
            _webClient.Dispose();
        }
    }
}