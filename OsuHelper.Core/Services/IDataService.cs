using System.Collections.Generic;
using System.Threading.Tasks;
using OsuHelper.Models;

namespace OsuHelper.Services
{
    public interface IDataService
    {
        Task<Beatmap> GetBeatmapAsync(string beatmapId, GameMode gameMode);

        Task<string> GetBeatmapRawAsync(string beatmapId);

        Task<IEnumerable<Play>> GetUserTopPlaysAsync(string userId, GameMode gameMode);

        Task<IEnumerable<Play>> GetBeatmapTopPlaysAsync(string beatmapId, GameMode gameMode, Mods mods);
    }
}