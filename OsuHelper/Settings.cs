// ------------------------------------------------------------------ 
//  Solution: <OsuHelper>
//  Project: <OsuHelper>
//  File: <Settings.cs>
//  Created By: Alexey Golub
//  Date: 20/08/2016
// ------------------------------------------------------------------ 

using NegativeLayer.Extensions;
using NegativeLayer.Settings;
using Newtonsoft.Json;
using OsuHelper.Models.API;
using OsuHelper.Models.Internal;

namespace OsuHelper
{
    public sealed class Settings : SettingsManager
    {
        public static Settings Default { get; } = new Settings();

        private string _userID;
        private APIProvider _apiProvider;
        private string _apiKey;

        private bool _preferNoVideo;
        private bool _onlyFullCombo = true;
        private int _ownPlayCountToScan = 20;
        private int _othersPlayCountToScan = 5;
        private int _similarPlayCount = 5;
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

        public int OwnPlayCountToScan
        {
            get { return _ownPlayCountToScan; }
            set { Set(ref _ownPlayCountToScan, value); }
        }

        public int OthersPlayCountToScan
        {
            get { return _othersPlayCountToScan; }
            set { Set(ref _othersPlayCountToScan, value); }
        }

        public int SimilarPlayCount
        {
            get { return _similarPlayCount; }
            set { Set(ref _similarPlayCount, value); }
        }

        public int RecommendationCount
        {
            get { return _recommendationCount; }
            set { Set(ref _recommendationCount, value); }
        }

        [JsonIgnore]
        public double DifficultyPreference
        {
            get { return 1.0 - OwnPlayCountToScan/100.0; }
            set { OwnPlayCountToScan = (int) (100.0*(1.0 - value)); }
        }

        public Settings()
            : base(new SettingsManagerConfiguration("OsuHelper"))
        {
            TryLoad();
        }
    }
}