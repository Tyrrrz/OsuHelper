using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using OsuHelper.Models;
using Tyrrrz.Extensions;

namespace OsuHelper.Services
{
    public class RecommendationService : IRecommendationService
    {
        private readonly IDataService _dataService;

        public RecommendationService(IDataService dataService)
        {
            if (dataService == null)
                throw new ArgumentNullException(nameof(dataService));

            _dataService = dataService;
        }

        public async Task<IEnumerable<BeatmapRecommendation>> GetRecommendationsAsync(GameMode gameMode, string userId, int maxCount)
        {
            if (userId == null)
                throw new ArgumentNullException(nameof(userId));
            if (maxCount < 0)
                throw new ArgumentOutOfRangeException(nameof(maxCount));

            // Get user's top plays
            var topPlays = (await _dataService.GetUserTopPlaysAsync(gameMode, userId))
                .OrderByDescending(p => p.PerformancePoints)
                .ToArray();

            // If no top plays - return empty
            if (!topPlays.Any()) return Enumerable.Empty<BeatmapRecommendation>();

            // Get similar players
            var similarPlayers = (await topPlays.ParallelSelectAsync(async play => await _dataService.GetBeatmapTopPlaysAsync(gameMode, play.BeatmapId, play.Mods)))
                .SelectMany(p => p) // Flatten
                .Select(p => p.PlayerId) // Select player ID
                .Distinct() // Only unique
                .Take(200)
                .ToArray();
            Debug.WriteLine($"Obtained IDs of {similarPlayers.Length} similar players", GetType().Name);

            // Get their top plays
            var ignoredBeatmaps = topPlays.Select(p => p.BeatmapId).ToArray();
            var similarTopPlays = (await similarPlayers.ParallelSelectAsync(async player => await _dataService.GetUserTopPlaysAsync(gameMode, player)))
                .SelectMany(p => p) // Flatten
                .Where(p => p.Rank >= PlayRank.S) // At least S rank;
                .Where(p => !p.BeatmapId.IsEither(ignoredBeatmaps)) // Not ignored beatmap
                .Take(200)
                .ToArray();
            Debug.WriteLine($"Obtained {similarTopPlays.Length} similar top plays", GetType().Name);

            // Prepare recommendations
            var potentialRecommendations = similarTopPlays
                .GroupBy(p => p.BeatmapId) // Group by beatmap
                .ToArray();
            Debug.WriteLine($"Obtained {potentialRecommendations.Length} potential recommendations", GetType().Name);

            // Assemble recommendations
            var result = (await potentialRecommendations.ParallelSelectAsync(async group =>
            {
                int count = group.Count();

                // Get median play based on PP
                var play = group.OrderBy(p => p.PerformancePoints).ElementAt(count/2);

                // Get beatmap data
                var beatmap = await _dataService.GetBeatmapAsync(gameMode, play.BeatmapId);

                // Add recommendation
                var recommendation = new BeatmapRecommendation();
                recommendation.Popularity = count;
                recommendation.Beatmap = beatmap;
                recommendation.Mods = play.Mods;
                recommendation.ExpectedAccuracy = play.Accuracy;
                recommendation.ExpectedPerformancePoints = play.PerformancePoints;

                return recommendation;
            }))
            .OrderByDescending(r => r.Popularity) // Order by popularity
            .Take(maxCount) // Limit to max count
            .ToArray();
            Debug.WriteLine($"Generated {result.Length} recommendations", GetType().Name);

            return result;
        }
    }
}