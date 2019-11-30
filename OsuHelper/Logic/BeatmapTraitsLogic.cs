using System;
using OsuHelper.Models;

namespace OsuHelper.Logic
{
    public static class BeatmapTraitsLogic
    {
        private static TimeSpan ApproachRateToTime(double ar)
        {
            return ar <= 5
                ? TimeSpan.FromMilliseconds(1800 - ar * 120)
                : TimeSpan.FromMilliseconds(1050 - (ar - 6) * 150);
        }

        private static double TimeToApproachRate(TimeSpan time)
        {
            var ms = time.TotalMilliseconds;
            return ms > 1200
                ? (1800 - ms) / 120
                : (1050 - ms) / 150 + 6;
        }

        private static TimeSpan OverallDifficultyToTime(double od)
        {
            return TimeSpan.FromMilliseconds(78 - od * 6);
        }

        private static double TimeToOverallDifficulty(TimeSpan time)
        {
            var ms = time.TotalMilliseconds;
            return (78 - ms) / 6;
        }

        private static TimeSpan DoubleTimeDuration(TimeSpan originalDuration) => originalDuration / 1.5;

        private static TimeSpan HalfTimeDuration(TimeSpan originalDuration) => originalDuration / 0.75;

        private static double DoubleTimeTempo(double originalTempo) => originalTempo * 1.5;

        private static double HalfTimeTempo(double originalTempo) => originalTempo * 0.75;

        private static double HardRockApproachRate(double originalApproachRate) => Math.Min(10, originalApproachRate * 1.4);

        private static double HardRockOverallDifficulty(double originalOverallDifficulty) => Math.Min(10, originalOverallDifficulty * 1.4);

        private static double HardRockCircleSize(double originalCircleSize) => Math.Min(10, originalCircleSize * 1.4);

        private static double HardRockDrain(double originalDrain) => Math.Min(10, originalDrain * 1.4);

        private static double EasyApproachRate(double originalApproachRate) => originalApproachRate / 2;

        private static double EasyOverallDifficulty(double originalOverallDifficulty) => originalOverallDifficulty / 2;

        private static double EasyCircleSize(double originalCircleSize) => originalCircleSize / 2;

        private static double EasyDrain(double originalDrain) => originalDrain / 2;

        private static double DoubleTimeApproachRate(double originalApproachRate) =>
            originalApproachRate
                .Pipe(ApproachRateToTime)
                .Pipe(DoubleTimeDuration)
                .Pipe(TimeToApproachRate);

        private static double HalfTimeApproachRate(double originalApproachRate) =>
            originalApproachRate
                .Pipe(ApproachRateToTime)
                .Pipe(HalfTimeDuration)
                .Pipe(TimeToApproachRate);

        private static double DoubleTimeOverallDifficulty(double originalApproachRate) =>
            originalApproachRate
                .Pipe(OverallDifficultyToTime)
                .Pipe(DoubleTimeDuration)
                .Pipe(TimeToOverallDifficulty);

        private static double HalfTimeOverallDifficulty(double originalApproachRate) =>
            originalApproachRate
                .Pipe(OverallDifficultyToTime)
                .Pipe(HalfTimeDuration)
                .Pipe(TimeToOverallDifficulty);

        private static TimeSpan AdjustDuration(TimeSpan originalDuration, Mods mods)
        {
            if (mods.HasFlag(Mods.DoubleTime))
                return DoubleTimeDuration(originalDuration);

            if (mods.HasFlag(Mods.HalfTime))
                return HalfTimeDuration(originalDuration);

            return originalDuration;
        }

        private static double AdjustTempo(double originalTempo, Mods mods)
        {
            if (mods.HasFlag(Mods.DoubleTime))
                return DoubleTimeTempo(originalTempo);

            if (mods.HasFlag(Mods.HalfTime))
                return HalfTimeTempo(originalTempo);

            return originalTempo;
        }

        private static double AdjustApproachRate(double originalApproachRate, Mods mods)
        {
            var approachRate = originalApproachRate;

            if (mods.HasFlag(Mods.HardRock))
                approachRate = HardRockApproachRate(approachRate);
            else if (mods.HasFlag(Mods.Easy))
                approachRate = EasyApproachRate(approachRate);

            if (mods.HasFlag(Mods.DoubleTime))
                approachRate = DoubleTimeApproachRate(approachRate);
            else if (mods.HasFlag(Mods.HalfTime))
                approachRate = HalfTimeApproachRate(approachRate);

            return approachRate;
        }

        private static double AdjustOverallDifficulty(double originalOverallDifficulty, Mods mods)
        {
            var overallDifficulty = originalOverallDifficulty;

            if (mods.HasFlag(Mods.HardRock))
                overallDifficulty = HardRockOverallDifficulty(overallDifficulty);
            else if (mods.HasFlag(Mods.Easy))
                overallDifficulty = EasyOverallDifficulty(overallDifficulty);

            if (mods.HasFlag(Mods.DoubleTime))
                overallDifficulty = DoubleTimeOverallDifficulty(overallDifficulty);
            else if (mods.HasFlag(Mods.HalfTime))
                overallDifficulty = HalfTimeOverallDifficulty(overallDifficulty);

            return overallDifficulty;
        }

        private static double AdjustCircleSize(double originalCircleSize, Mods mods)
        {
            if (mods.HasFlag(Mods.HardRock))
                return HardRockCircleSize(originalCircleSize);

            if (mods.HasFlag(Mods.Easy))
                return EasyCircleSize(originalCircleSize);

            return originalCircleSize;
        }

        private static double AdjustDrain(double originalDrain, Mods mods)
        {
            if (mods.HasFlag(Mods.HardRock))
                return HardRockDrain(originalDrain);

            if (mods.HasFlag(Mods.Easy))
                return EasyDrain(originalDrain);

            return originalDrain;
        }

        public static BeatmapTraits CalculateTraitsWithMods(BeatmapTraits nomodTraits, Mods mods, GameMode mode)
        {
            // No mods - just return base traits
            if (mods == Mods.None)
                return nomodTraits;

            // Not standard - return base traits (no idea how those mods work)
            if (mode != GameMode.Standard)
                return nomodTraits;

            return new BeatmapTraits(
                nomodTraits.MaxCombo, // doesn't change
                AdjustDuration(nomodTraits.Duration, mods),
                AdjustTempo(nomodTraits.Tempo, mods),
                nomodTraits.StarRating, // can't calculate
                AdjustApproachRate(nomodTraits.ApproachRate, mods),
                AdjustOverallDifficulty(nomodTraits.OverallDifficulty, mods),
                AdjustCircleSize(nomodTraits.CircleSize, mods),
                AdjustDrain(nomodTraits.Drain, mods)
            );
        }
    }
}