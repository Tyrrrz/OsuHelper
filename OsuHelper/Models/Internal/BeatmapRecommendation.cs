// ------------------------------------------------------------------ 
//  Solution: <OsuHelper>
//  Project: <OsuHelper>
//  File: <BeatmapRecommendation.cs>
//  Created By: Alexey Golub
//  Date: 20/08/2016
// ------------------------------------------------------------------ 

using System.Collections.Generic;
using NegativeLayer.Extensions;
using OsuHelper.Models.API;

namespace OsuHelper.Models.Internal
{
    public class BeatmapRecommendation
    {
        public Beatmap Beatmap { get; }

        public double ExpectedPerformancePoints { get; }

        public double ExpectedAccuracy { get; }

        public EnabledMods Mods { get; }

        public string ModsString
        {
            get
            {
                if (Mods == EnabledMods.None) return string.Empty;

                var mods = new List<string>();
                // Only mods that influence PP and/or are ranked
                if (Mods.HasFlag(EnabledMods.NoFail))
                    mods.Add("NF");
                if (Mods.HasFlag(EnabledMods.Easy))
                    mods.Add("EZ");
                if (Mods.HasFlag(EnabledMods.Hidden))
                    mods.Add("HD");
                if (Mods.HasFlag(EnabledMods.HardRock))
                    mods.Add("HR");
                if (Mods.HasFlag(EnabledMods.DoubleTime) || Mods.HasFlag(EnabledMods.Nightcore))
                    mods.Add("DT");
                if (Mods.HasFlag(EnabledMods.HalfTime))
                    mods.Add("HT");
                if (Mods.HasFlag(EnabledMods.Flashlight))
                    mods.Add("FL");
                if (Mods.HasFlag(EnabledMods.SpunOut))
                    mods.Add("SO");

                return mods.JoinToString();
            }
        }

        public BeatmapRecommendation(Beatmap beatmap, double expectedPerformancePoints, double expectedAccuracy,
            EnabledMods mods)
        {
            Beatmap = beatmap;
            ExpectedPerformancePoints = expectedPerformancePoints;
            ExpectedAccuracy = expectedAccuracy;
            Mods = mods;
        }
    }
}