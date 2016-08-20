// ------------------------------------------------------------------ 
//  Solution: <OsuHelper>
//  Project: <OsuHelper>
//  File: <Play.cs>
//  Created By: Alexey Golub
//  Date: 20/08/2016
// ------------------------------------------------------------------ 

// ReSharper disable UnusedAutoPropertyAccessor.Local (serialization)

using System.Collections.Generic;
using NegativeLayer.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace OsuHelper.Models.API
{
    public class Play
    {
        [JsonProperty("beatmap_id")]
        public string BeatmapID { get; private set; }

        [JsonProperty("user_id")]
        public string UserID { get; private set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public PlayRank Rank { get; private set; }

        public int Count300 { get; private set; }
        public int Count100 { get; private set; }
        public int Count50 { get; private set; }
        public int CountMiss { get; private set; }

        public double Accuracy
            => (Count50*50.0 + Count100*100.0 + Count300*300.0)/(Count50 + Count100 + Count300 + CountMiss);

        public int MaxCombo { get; private set; }

        [JsonProperty("pp")]
        public double PerformancePoints { get; private set; }

        public EnabledMods ModsUsed { get; private set; }

        public string ModsUsedString 
        {
            get
            {
                if (ModsUsed == EnabledMods.None) return string.Empty;

                var mods = new List<string>();
                // Only mods that influence PP and/or are ranked
                if (ModsUsed.HasFlag(EnabledMods.NoFail))
                    mods.Add("NF");
                if (ModsUsed.HasFlag(EnabledMods.Easy))
                    mods.Add("EZ");
                if (ModsUsed.HasFlag(EnabledMods.Hidden))
                    mods.Add("HD");
                if (ModsUsed.HasFlag(EnabledMods.HardRock))
                    mods.Add("HR");
                if (ModsUsed.HasFlag(EnabledMods.DoubleTime) || ModsUsed.HasFlag(EnabledMods.Nightcore))
                    mods.Add("DT");
                if (ModsUsed.HasFlag(EnabledMods.HalfTime))
                    mods.Add("HT");
                if (ModsUsed.HasFlag(EnabledMods.Flashlight))
                    mods.Add("FL");
                if (ModsUsed.HasFlag(EnabledMods.SpunOut))
                    mods.Add("SO");

                return mods.JoinToString();
            }
        }

        private Play() { }
    }
}