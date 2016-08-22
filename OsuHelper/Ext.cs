// ------------------------------------------------------------------ 
//  Solution: <OsuHelper>
//  Project: <OsuHelper>
//  File: <Ext.cs>
//  Created By: Alexey Golub
//  Date: 22/08/2016
// ------------------------------------------------------------------ 

using System.Collections.Generic;
using NegativeLayer.Extensions;
using OsuHelper.Models.API;

namespace OsuHelper
{
    public static class Ext
    {
        public static string GetModsString(this EnabledMods mods)
        {
            if (mods == EnabledMods.None) return string.Empty;

            var modsBuffer = new List<string>();

            // Only mods that influence PP and/or are ranked
            if (mods.HasFlag(EnabledMods.NoFail))
                modsBuffer.Add("NF");
            if (mods.HasFlag(EnabledMods.Easy))
                modsBuffer.Add("EZ");
            if (mods.HasFlag(EnabledMods.Hidden))
                modsBuffer.Add("HD");
            if (mods.HasFlag(EnabledMods.HardRock))
                modsBuffer.Add("HR");
            if (mods.HasFlag(EnabledMods.DoubleTime) || mods.HasFlag(EnabledMods.Nightcore))
                modsBuffer.Add("DT");
            if (mods.HasFlag(EnabledMods.HalfTime))
                modsBuffer.Add("HT");
            if (mods.HasFlag(EnabledMods.Flashlight))
                modsBuffer.Add("FL");
            if (mods.HasFlag(EnabledMods.SpunOut))
                modsBuffer.Add("SO");

            return modsBuffer.JoinToString("");
        }
    }
}