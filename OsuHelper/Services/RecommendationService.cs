using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OsuHelper.Models;
using Tyrrrz.Extensions;

namespace OsuHelper.Services
{
    public class RecommendationService : IRecommendationService
    {
        private readonly ISettingsService _settingsService;
        private readonly IDataService _dataService;
        private readonly IBeatmapProcessorService _beatmapProcessorService;

        private string UserId => _settingsService.UserId;
        private GameMode GameMode => _settingsService.GameMode;

        public RecommendationService(ISettingsService settingsService, IDataService dataService,
            IBeatmapProcessorService beatmapProcessorService)
        {
            _settingsService = settingsService;
            _dataService = dataService;
            _beatmapProcessorService = beatmapProcessorService;
        }

        public async Task<IReadOnlyList<BeatmapRecommendation>> GetRecommendationsAsync()
        {
            // Get user's top plays
            var ownTopPlays = (await _dataService.GetUserTopPlaysAsync(UserId, GameMode))
                .OrderByDescending(p => p.PerformancePoints)
                .ToArray();

            // If no top plays - return empty
            if (!ownTopPlays.Any())
                return new BeatmapRecommendation[0];

            // Get maps where they were made
            var ownTopMaps = ownTopPlays
                .Select(p => p.BeatmapId)
                .ToArray();

            // Set boundaries
            var minPP = ownTopPlays.Take(15).Average(p => p.PerformancePoints);
            var maxPP = ownTopPlays.Take(15).Average(p => p.PerformancePoints)*1.25;

            // Prepare buffer for recommendation bases
            var recommendationBases = new List<Play>();

            // Go through top X plays
            await ownTopPlays.Take(15).ParallelForEachAsync(async ownTopPlay =>
            {
                // Get the map's top plays
                var mapTopPlays =
                    (await _dataService.GetBeatmapTopPlaysAsync(ownTopPlay.BeatmapId, GameMode, ownTopPlay.Mods))
                    .OrderBy(p => Math.Abs(p.PerformancePoints - ownTopPlay.PerformancePoints))
                    .Take(10);

                // Go through those top plays
                await mapTopPlays.ParallelForEachAsync(async mapTopPlay =>
                {
                    // Get top plays of that user
                    var otherUserTopPlays = (await _dataService.GetUserTopPlaysAsync(mapTopPlay.PlayerId, GameMode))
                        .Where(p => p.Rank >= PlayRank.S)
                        .Where(p => p.PerformancePoints >= minPP)
                        .Where(p => p.PerformancePoints <= maxPP)
                        .OrderBy(p => Math.Abs(p.PerformancePoints - ownTopPlay.PerformancePoints))
                        .Take(10);

                    recommendationBases.AddRange(otherUserTopPlays);
                });
            });

            // Prepare recommendation groups
            var recommendationGroups = recommendationBases
                .GroupBy(p => p.BeatmapId)
                .Where(g => !g.Key.IsEither(ownTopMaps))
                .OrderByDescending(g => g.Count())
                .Take(100);

            // Assemble recommendations
            var result = new List<BeatmapRecommendation>();
            await recommendationGroups.ParallelForEachAsync(async group =>
            {
                var count = group.Count();

                // Get median play based on PP
                var play = group.OrderBy(p => p.PerformancePoints).ElementAt(count/2);

                // Get beatmap data
                var beatmap = await _dataService.GetBeatmapAsync(play.BeatmapId, GameMode);

                // Calculate traits with mods
                var traitsWithMods = _beatmapProcessorService.CalculateTraitsWithMods(beatmap, play.Mods);

                // Add recommendation
                var recommendation = new BeatmapRecommendation(beatmap, count, play.Mods, traitsWithMods, play.Accuracy, play.PerformancePoints);
                result.Add(recommendation);
            });

            return result.OrderByDescending(r => r.Weight).ToArray();
        }
    }
}