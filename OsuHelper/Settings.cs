// ------------------------------------------------------------------ 
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
        private string _userID;
        private string _apiKey;

        public static Settings Default { get; } = new Settings();

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

        public Settings()
        {
            // Load settings on initialization
            TryLoad();
        }
    }
}