using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using OsuHelper.Logic;
using OsuHelper.Models;

namespace OsuHelper.Tests.Logic
{
    [TestFixture]
    public class BeatmapTraitsLogicTests
    {
        private static IEnumerable<TestCaseData> GetTestCases_CalculateTraitsWithMods()
        {
            // Nomod
            yield return new TestCaseData(
                // Input
                new BeatmapTraits(
                    100,
                    new TimeSpan(00, 01, 30),
                    200, 5, 9, 8, 8, 7
                ),
                Mods.None,
                GameMode.Standard,

                // Output
                new BeatmapTraits(
                    100,
                    new TimeSpan(00, 01, 30),
                    200, 5, 9, 8, 8, 7
                )
            );

            // Hardrock
            yield return new TestCaseData(
                // Input
                new BeatmapTraits(
                    100,
                    new TimeSpan(00, 01, 30),
                    200, 5, 9, 8, 8, 7
                ),
                Mods.HardRock,
                GameMode.Standard,

                // Output
                new BeatmapTraits(
                    100,
                    new TimeSpan(00, 01, 30),
                    200, 5, 10, 10, 10, 9.8
                )
            );

            // Easy
            yield return new TestCaseData(
                // Input
                new BeatmapTraits(
                    100,
                    new TimeSpan(00, 01, 30),
                    200, 5, 9, 8, 8, 7
                ),
                Mods.Easy,
                GameMode.Standard,

                // Output
                new BeatmapTraits(
                    100,
                    new TimeSpan(00, 01, 30),
                    200, 5, 4.5, 4, 4, 3.5
                )
            );

            // DoubleTime
            yield return new TestCaseData(
                // Input
                new BeatmapTraits(
                    100,
                    new TimeSpan(00, 01, 30),
                    200, 5, 9, 8, 8, 7
                ),
                Mods.DoubleTime,
                GameMode.Standard,

                // Output
                new BeatmapTraits(
                    100,
                    new TimeSpan(00, 01, 00),
                    300, 5, 10.33, 9.67, 8, 7
                )
            );

            // HalfTime
            yield return new TestCaseData(
                // Input
                new BeatmapTraits(
                    100,
                    new TimeSpan(00, 01, 30),
                    200, 5, 9, 8, 8, 7
                ),
                Mods.HalfTime,
                GameMode.Standard,

                // Output
                new BeatmapTraits(
                    100,
                    new TimeSpan(00, 02, 00),
                    150, 5, 7.67, 6.33, 8, 7
                )
            );

            // DoubleTime + HardRock
            yield return new TestCaseData(
                // Input
                new BeatmapTraits(
                    100,
                    new TimeSpan(00, 01, 30),
                    200, 5, 9, 8, 8, 7
                ),
                Mods.DoubleTime | Mods.HardRock,
                GameMode.Standard,

                // Output
                new BeatmapTraits(
                    100,
                    new TimeSpan(00, 01, 00),
                    300, 5, 11, 11, 10, 9.8
                )
            );

            // DoubleTime + Easy
            yield return new TestCaseData(
                // Input
                new BeatmapTraits(
                    100,
                    new TimeSpan(00, 01, 30),
                    200, 5, 9, 8, 8, 7
                ),
                Mods.DoubleTime | Mods.Easy,
                GameMode.Standard,

                // Output
                new BeatmapTraits(
                    100,
                    new TimeSpan(00, 01, 00),
                    300, 5, 7.4, 7, 4, 3.5
                )
            );

            // HalfTime + HardRock
            yield return new TestCaseData(
                // Input
                new BeatmapTraits(
                    100,
                    new TimeSpan(00, 01, 30),
                    200, 5, 9, 8, 8, 7
                ),
                Mods.HalfTime | Mods.HardRock,
                GameMode.Standard,

                // Output
                new BeatmapTraits(
                    100,
                    new TimeSpan(00, 02, 00),
                    150, 5, 9, 9, 10, 9.8
                )
            );

            // HalfTime + Easy
            yield return new TestCaseData(
                // Input
                new BeatmapTraits(
                    100,
                    new TimeSpan(00, 01, 30),
                    200, 5, 9, 8, 8, 7
                ),
                Mods.HalfTime | Mods.Easy,
                GameMode.Standard,

                // Output
                new BeatmapTraits(
                    100,
                    new TimeSpan(00, 02, 00),
                    150, 5, 1, 1, 4, 3.5
                )
            );
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_CalculateTraitsWithMods))]
        public void CalculateTraitsWithMods(BeatmapTraits nomodTraits, Mods mods, GameMode mode, BeatmapTraits expectedTraits)
        {
            // Act
            var traits = BeatmapTraitsLogic.CalculateTraitsWithMods(nomodTraits, mods, mode);

            // Assert
            traits.Should().BeEquivalentTo(expectedTraits,
                o => o.Using<double>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, 0.01)).WhenTypeIs<double>());
        }
    }
}