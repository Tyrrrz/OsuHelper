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
                throw new ArgumentOutOfRangeException(nameof(userId));

            // Get user's top plays
            var plays = (await _dataService.GetUserTopPlaysAsync(gameMode, userId))
                .OrderByDescending(p => p.PerformancePoints)
                .ToArray();
            Debug.WriteLine($"Obtained {plays.Length} user top plays", GetType().Name);

            // If no top plays - return empty
            if (!plays.Any()) return Enumerable.Empty<BeatmapRecommendation>();

            // Get players who also have a play on that map
            var similarPlayers = new List<string>();
            await plays.ParallelForEachAsync(async play =>
            {
                // Get map's top plays
                var topPlays =
                    await _dataService.GetBeatmapTopPlaysAsync(gameMode, play.BeatmapId, play.Mods);

                // Only plays worth within 20% pp
                topPlays = topPlays
                    .Where(p => Math.Abs(p.PerformancePoints - play.PerformancePoints)/play.PerformancePoints <= 0.2);

                // Buffer
                topPlays = topPlays.ToArray();

                // If no top plays - return
                if (!topPlays.Any()) return;

                // Add to list
                foreach (string id in topPlays.Select(p => p.PlayerId))
                    similarPlayers.AddIfDistinct(id);
            });
            Debug.WriteLine($"Obtained IDs of {similarPlayers.Count} similar players", GetType().Name);

            // Get their top plays
            var potentialRecommendations = new List<Play>();
            var ignoredBeatmaps = plays.Select(p => p.BeatmapId).ToArray();
            await similarPlayers.ParallelForEachAsync(async player =>
            {
                // Get player's top plays
                var topPlays =
                    await _dataService.GetUserTopPlaysAsync(gameMode, player);

                // Filter out ignored maps
                topPlays = topPlays.Where(p => !p.BeatmapId.IsEither(ignoredBeatmaps));

                // Only at least S ranks
                topPlays = topPlays.Where(p => p.Rank >= PlayRank.S);

                // Buffer
                topPlays = topPlays.ToArray();

                // If no top plays - return
                if (!topPlays.Any()) return;

                // Add to list
                foreach (var play in topPlays)
                    potentialRecommendations.AddIfDistinct(play);

            });
            var potentialRecommendationsGroups = potentialRecommendations.GroupBy(p => p.BeatmapId).ToArray();
            Debug.WriteLine($"Obtained {potentialRecommendationsGroups.Length} potential recommendations", GetType().Name);

            // Assemble recommendations
            var result = new List<BeatmapRecommendation>();
            await potentialRecommendationsGroups.ParallelForEachAsync(async group =>
            {
                int count = group.Count();

                // Get median play based on PP
                Play play;
                if (count == 1)
                {
                    play = group.First();
                }
                else
                {
                    var ordered = group.OrderBy(p => p.PerformancePoints);
                    int mid = count/2;
                    play = ordered.ElementAt(mid);
                }

                // Get beatmap data
                var beatmap = await _dataService.GetBeatmapAsync(gameMode, play.BeatmapId);

                // Add recommendation
                var recommendation = new BeatmapRecommendation();
                recommendation.Popularity = count;
                recommendation.Beatmap = beatmap;
                recommendation.Mods = play.Mods;
                recommendation.ExpectedAccuracy = play.Accuracy;
                recommendation.ExpectedPerformancePoints = play.PerformancePoints;

                result.Add(recommendation);
            });

            return result;
        }
    }
}