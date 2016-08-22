// ------------------------------------------------------------------ 
//  Solution: <OsuHelper>
//  Project: <OsuHelper>
//  File: <Ext.cs>
//  Created By: Alexey Golub
//  Date: 22/08/2016
// ------------------------------------------------------------------ 

using System;
using System.Collections.Generic;
using System.Linq;
using NegativeLayer.Extensions;
using OsuHelper.Models.API;

namespace OsuHelper
{
    public static class Ext
    {
        private static readonly List<Tuple<double, double>> ApproachRateTable = new List<Tuple<double, double>>
        {
            Tuple.Create<double, double>(0, 1800),
            Tuple.Create<double, double>(1, 1680),
            Tuple.Create<double, double>(2, 1560),
            Tuple.Create<double, double>(3, 1440),
            Tuple.Create<double, double>(4, 1320),
            Tuple.Create<double, double>(5, 1200),
            Tuple.Create<double, double>(6, 1050),
            Tuple.Create<double, double>(7, 900),
            Tuple.Create<double, double>(8, 750),
            Tuple.Create<double, double>(9, 600),
            Tuple.Create<double, double>(10, 450),
            Tuple.Create<double, double>(11, 300)
        };

        private static readonly List<Tuple<double, double>> OverallDifficultyTable = new List<Tuple<double, double>>
        {
            Tuple.Create<double, double>(0, 78),
            Tuple.Create<double, double>(1, 72),
            Tuple.Create<double, double>(2, 66),
            Tuple.Create<double, double>(3, 60),
            Tuple.Create<double, double>(4, 54),
            Tuple.Create<double, double>(5, 48),
            Tuple.Create<double, double>(6, 42),
            Tuple.Create<double, double>(7, 36),
            Tuple.Create<double, double>(8, 30),
            Tuple.Create<double, double>(9, 24),
            Tuple.Create<double, double>(10, 18),
            Tuple.Create<double, double>(11, 12)
        };

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

        public static double CalculateDoubleTimeApproachRate(double normalApproachRate)
        {
            normalApproachRate = normalApproachRate.Clamp(0, 10);

            // Get the ms time value of this AR
            double time;
            if (ApproachRateTable.Any(t => t.Item1.Equals(normalApproachRate)))
            {
                time = ApproachRateTable.First(t => t.Item1.Equals(normalApproachRate)).Item2;
            }
            else
            {
                // If exact match not found - linearly approximate
                var closestLower = ApproachRateTable.Last(t => t.Item1 <= normalApproachRate);
                double closestLowerAr = closestLower.Item1;
                double closestLowerTime = closestLower.Item2;
                time = closestLowerTime*closestLowerAr/normalApproachRate;
            }

            // Multiply ms
            time /= 1.5;

            // Try to find exact values
            if (ApproachRateTable.Any(t => t.Item2.Equals(time)))
                return ApproachRateTable.First(t => t.Item2.Equals(time)).Item1;

            // Try to find closest values
            var lower = ApproachRateTable.Last(t => t.Item2 >= time);
            var higher = ApproachRateTable.First(t => t.Item2 <= time);
            double fullDelta = lower.Item2 - higher.Item2;
            double delta = lower.Item2 - time;
            return lower.Item1 + delta/fullDelta;
        }

        public static double CalculateDoubleTimeOverallDifficulty(double normalOverallDifficulty)
        {
            normalOverallDifficulty = normalOverallDifficulty.Clamp(0, 10);

            // Get the ms time value of this OD
            double time;
            if (OverallDifficultyTable.Any(t => t.Item1.Equals(normalOverallDifficulty)))
            {
                time = OverallDifficultyTable.First(t => t.Item1.Equals(normalOverallDifficulty)).Item2;
            }
            else
            {
                // If exact match not found - linearly approximate
                var closestLower = OverallDifficultyTable.Last(t => t.Item1 <= normalOverallDifficulty);
                double closestLowerAr = closestLower.Item1;
                double closestLowerTime = closestLower.Item2;
                time = closestLowerTime*closestLowerAr/normalOverallDifficulty;
            }

            // Multiply ms
            time /= 1.5;

            // Try to find exact values
            if (OverallDifficultyTable.Any(t => t.Item2.Equals(time)))
                return OverallDifficultyTable.First(t => t.Item2.Equals(time)).Item1;

            // Try to find closest values
            var lower = OverallDifficultyTable.Last(t => t.Item2 >= time);
            var higher = OverallDifficultyTable.First(t => t.Item2 <= time);
            double fullDelta = lower.Item2 - higher.Item2;
            double delta = lower.Item2 - time;
            return lower.Item1 + delta/fullDelta;
        }
    }
}