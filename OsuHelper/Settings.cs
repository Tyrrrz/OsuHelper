﻿// ------------------------------------------------------------------ 
//  Solution: <OsuHelper>
//  Project: <OsuHelper>
//  File: <Settings.cs>
//  Created By: Alexey Golub
//  Date: 20/08/2016
// ------------------------------------------------------------------ 

using NegativeLayer.Settings;

namespace OsuHelper
{
    public sealed class Settings : SettingsManager
    {
        public static Settings Default { get; } = new Settings();

        private string _userID;
        private string _apiKey;
        private int _ownPlayCountToScan = 20;
        private int _othersPlayCountToScan = 5;
        private int _similarPlayCount = 5;
        private int _recommendationCount = 200;

        public string UserID
        {
            get { return _userID; }
            set { Set(ref _userID, value); }
        }

        public string APIKey
        {
            get { return _apiKey; }
            set { Set(ref _apiKey, value); }
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

        public Settings()
            : base(new SettingsManagerConfiguration("OsuHelper"))
        {
            TryLoad();
        }
    }
}