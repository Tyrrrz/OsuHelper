using System;
using OsuHelper.Models;
using Tyrrrz.Extensions;

namespace OsuHelper.Services
{
    public class BeatmapProcessorService
    {
        private TimeSpan ApproachRateToTime(double ar)
        {
            return ar <= 5
                ? TimeSpan.FromMilliseconds(1800 - ar * 120)
                : TimeSpan.FromMilliseconds(1050 - (ar - 6) * 150);
        }

        private double TimeToApproachRate(TimeSpan time)
        {
            var ms = time.TotalMilliseconds;
            return ms > 1200
                ? (1800 - ms) / 120
                : (1050 - ms) / 150 + 6;
        }

        private TimeSpan OverallDifficultyToTime(double od)
        {
            return TimeSpan.FromMilliseconds(78 - od * 6);
        }

        private double TimeToOverallDifficulty(TimeSpan time)
        {
            var ms = time.TotalMilliseconds;
            return (78 - ms) / 6;
        }

        public BeatmapTraits CalculateBeatmapTraitsWithMods(Beatmap beatmap, Mods mods)
        {
            // No mods - just return base traits
            if (mods == Mods.None)
                return beatmap.Traits;

            // Not standard - return base traits (no idea how those mods work)
            if (beatmap.GameMode != GameMode.Standard)
                return beatmap.Traits;

            // Carry over unchanged traits
            var maxCombo = beatmap.Traits.MaxCombo;
            var sr = beatmap.Traits.StarRating; // can't calculate this

            // Calculate duration and tempo
            var duration = beatmap.Traits.Duration;
            var tempo = beatmap.Traits.Tempo;

            if (mods.HasFlag(Mods.DoubleTime))
            {
                duration = TimeSpan.FromSeconds(beatmap.Traits.Duration.Seconds / 1.5);
                tempo = beatmap.Traits.Tempo * 1.5;
            }
            else if (mods.HasFlag(Mods.HalfTime))
            {
                duration = TimeSpan.FromSeconds(beatmap.Traits.Duration.Seconds / 0.75);
                tempo = beatmap.Traits.Tempo * 0.75;
            }

            // Calculate AR and OD
            var ar = beatmap.Traits.ApproachRate;
            var od = beatmap.Traits.OverallDifficulty;

            if (mods.HasFlag(Mods.HardRock))
            {
                ar = (ar * 1.4).ClampMax(10);
                od = (od * 1.4).ClampMax(10);
            }
            else if (mods.HasFlag(Mods.Easy))
            {
                ar = ar / 2;
                od = od / 2;
            }

            if (mods.HasFlag(Mods.DoubleTime))
            {
                ar = TimeToApproachRate(TimeSpan.FromSeconds(ApproachRateToTime(ar).Seconds / 1.5));
                od = TimeToOverallDifficulty(TimeSpan.FromSeconds(OverallDifficultyToTime(od).Seconds / 1.5));
            }
            else if (mods.HasFlag(Mods.HalfTime))
            {
                ar = TimeToApproachRate(TimeSpan.FromSeconds(ApproachRateToTime(ar).Seconds / 0.75));
                od = TimeToOverallDifficulty(TimeSpan.FromSeconds(OverallDifficultyToTime(od).Seconds / 0.75));
            }

            // Calculate CS and HP
            var cs = beatmap.Traits.CircleSize;
            var hp = beatmap.Traits.Drain;

            if (mods.HasFlag(Mods.HardRock))
            {
                cs = (cs * 1.4).ClampMax(10);
                hp = (hp * 1.4).ClampMax(10);
            }
            else if (mods.HasFlag(Mods.Easy))
            {
                cs = cs / 2;
                hp = hp / 2;
            }

            return new BeatmapTraits(maxCombo, duration, tempo, sr, ar, od, cs, hp);
        }
    }
}