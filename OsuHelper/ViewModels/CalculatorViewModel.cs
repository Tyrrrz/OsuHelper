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
            set
            {
                Set(ref _mods, value);
                RaisePropertyChanged(() => HrEnabled);
                RaisePropertyChanged(() => DtEnabled);
                RaisePropertyChanged(() => HdEnabled);
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

        private async Task DownloadMap()
        {
            // Parse out the ID if needed
            var match = Regex.Match(BeatmapID, @".*?osu.ppy.sh/\w/(\d+)");
            if (match.Success)
                BeatmapID = match.Groups[1].Value;

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
            Update();

            CanAnalyze = true;
        }

        private async void Update()
        {
            // Make sure the map file is still there
            if (_beatmapFilePath.IsBlank() || !File.Exists(_beatmapFilePath))
                return;

            ExpectedPerformancePoints = await _oppaiService.CalculatePerformancePointsAsync(_beatmapFilePath, ExpectedAccuracy, Mods);
        }

        public void Dispose()
        {
            _webClient.Dispose();
        }
    }
}