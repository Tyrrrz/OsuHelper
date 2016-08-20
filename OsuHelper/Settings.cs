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
    public class Settings : SettingsManager
    {
        public static Settings Default { get; } = new Settings();

        public string UserName { get; set; }
        public string APIKey { get; set; }
    }
}