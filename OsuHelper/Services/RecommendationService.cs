using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OsuHelper.Exceptions;
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

        public async Task<IReadOnlyList<Recommendation>> GetRecommendationsAsync()
        {
            // Get user's top plays
            var ownTopPlays = (await _dataService.GetUserTopPlaysAsync(UserId, GameMode))
                .OrderByDescending(p => p.PerformancePoints) // sort by PP
                .ToArray();

            // If user doesn't have any plays - throw
            if (!ownTopPlays.Any())
                throw new RecommendationsUnavailableException("User hasn't set any scores in given game mode.");

            // Set boundaries for recommendations based on PP
            var minPP = ownTopPlays.Take(20).Average(p => p.PerformancePoints);
            var maxPP = minPP*1.25;

            // Prepare buffer for plays which will serve as base for recommendations
            var candidatePlays = new List<Play>();

            // Go through user's top 20 plays
            await ownTopPlays.Take(20).ParallelForEachAsync(async ownTopPlay =>
            {
                // Get the map's top plays
                var mapTopPlays =
                    (await _dataService.GetBeatmapTopPlaysAsync(ownTopPlay.BeatmapId, GameMode, ownTopPlay.Mods))
                    .OrderBy(p => Math.Abs(p.PerformancePoints - ownTopPlay.PerformancePoints)) // order by PP similarity
                    .Take(10); // only take top 10

                // Go through those top plays
                await mapTopPlays.ParallelForEachAsync(async mapTopPlay =>
                {
                    // Get top plays of that user
                    var otherUserTopPlays = (await _dataService.GetUserTopPlaysAsync(mapTopPlay.PlayerId, GameMode))
                        .Where(p => p.Rank >= PlayRank.S) // only S ranks
                        .Where(p => p.PerformancePoints >= minPP) // limit by minPP
                        .Where(p => p.PerformancePoints <= maxPP) // limit by maxPP
                        .OrderBy(p => Math.Abs(p.PerformancePoints - ownTopPlay.PerformancePoints)) // order by PP similarity
                        .Take(10);

                    // Add these plays to candidates
                    candidatePlays.AddRange(otherUserTopPlays);
                });
            });

            // Group candidate plays by beatmap
            var candidatePlaysGroups = candidatePlays
                .GroupBy(p => p.BeatmapId) // group
                .Where(g => !ownTopPlays.Select(p => p.BeatmapId).Contains(g.Key)) // filter out maps that the user has top plays on
                .OrderByDescending(g => g.Count()) // sort by number of times the map appears in candidate plays
                .Take(100); // only take top 100

            // Assemble recommendations
            var result = new List<Recommendation>();
            await candidatePlaysGroups.ParallelForEachAsync(async group =>
            {
                var count = group.Count();

                // Get median play based on PP
                var play = group.OrderBy(p => p.PerformancePoints).ElementAt(count/2);

                // Get beatmap data
                var beatmap = await _dataService.GetBeatmapAsync(play.BeatmapId, GameMode);

                // Calculate traits with mods
                var traitsWithMods = _beatmapProcessorService.CalculateTraitsWithMods(beatmap, play.Mods);

                // Add recommendation to the list
                var recommendation = new Recommendation(beatmap, count, play.Mods, traitsWithMods, play.Accuracy, play.PerformancePoints);
                result.Add(recommendation);
            });

            // Return recommendations sorted by weight
            return result.OrderByDescending(r => r.Weight).ToArray();
        }
    }
}