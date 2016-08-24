// ------------------------------------------------------------------ 
//  Solution: <OsuHelper>
//  Project: <OsuHelper>
//  File: <Settings.cs>
//  Created By: Alexey Golub
//  Date: 20/08/2016
// ------------------------------------------------------------------ 

using NegativeLayer.Settings;
using Newtonsoft.Json;
using OsuHelper.Models.API;
using OsuHelper.Models.Internal;

namespace OsuHelper
{
    public sealed class Settings : SettingsManager
    {
        public static SettingsManagerStager<Settings> Stager { get; } = new SettingsManagerStager<Settings>();

        static Settings()
        {
            Stager.TryLoad();
        }

        private string _userID;
        private APIProvider _apiProvider;
        private string _apiKey;
        private GameMode _gameMode = GameMode.Standard;

        private bool _preferNoVideo;
        private bool _onlyFullCombo = true;
        private double _previewSoundVolume = 0.8;
        private double _difficultyPreference = 0.5;
        private int _ownPlayCountToScan;
        private int _othersPlayCountToScan;
        private int _similarPlayCount;
        private int _recommendationCount = 200;

        public string UserID
        {
            get { return _userID; }
            set { Set(ref _userID, value); }
        }

        public APIProvider APIProvider
        {
            get { return _apiProvider; }
            set { Set(ref _apiProvider, value); }
        }

        public string APIKey
        {
            get { return _apiKey; }
            set { Set(ref _apiKey, value); }
        }

        public GameMode GameMode
        {
            get { return _gameMode; }
            set { Set(ref _gameMode, value); }
        }

        public bool PreferNoVideo
        {
            get { return _preferNoVideo; }
            set { Set(ref _preferNoVideo, value); }
        }

        public bool OnlyFullCombo
        {
            get { return _onlyFullCombo; }
            set { Set(ref _onlyFullCombo, value); }
        }

        public double PreviewSoundVolume
        {
            get { return _previewSoundVolume; }
            set { Set(ref _previewSoundVolume, value); }
        }

        public double DifficultyPreference
        {
            get { return _difficultyPreference; }
            set
            {
                Set(ref _difficultyPreference, value);
                UpdateDifficultyPreference();
            }
        }

        [JsonIgnore]
        public int OwnPlayCountToScan
        {
            get { return _ownPlayCountToScan; }
            private set { Set(ref _ownPlayCountToScan, value); }
        }

        [JsonIgnore]
        public int OthersPlayCountToScan
        {
            get { return _othersPlayCountToScan; }
            private set { Set(ref _othersPlayCountToScan, value); }
        }

        [JsonIgnore]
        public int SimilarPlayCount
        {
            get { return _similarPlayCount; }
            private set { Set(ref _similarPlayCount, value); }
        }

        public int RecommendationCount
        {
            get { return _recommendationCount; }
            set { Set(ref _recommendationCount, value); }
        }

        private void UpdateDifficultyPreference()
        {
            // 0 -> easy maps, 1 -> hard maps
            // 0 -> own play count = 35, others play count = 5, similar play count = 5
            // 1 -> own play count = 5, others play count = 20, similar play count = 20
            OwnPlayCountToScan = (int) (5 + ((1 - DifficultyPreference)*30));
            OthersPlayCountToScan = (int) (20 - ((1 - DifficultyPreference)*15));
            SimilarPlayCount = (int) (20 - ((1 - DifficultyPreference)*15));
        }

        public Settings()
            : base(new SettingsManagerConfiguration("OsuHelper"))
        {
        }
    }
}