using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using OsuHelper.Models;

namespace OsuHelper.Services
{
    public interface IDataService
    {
        Task<Beatmap> GetBeatmapAsync(string beatmapId, GameMode gameMode);

        Task<string> GetBeatmapRawAsync(string beatmapId);

        Task<Stream> GetBeatmapSetPreviewAsync(string mapSetId);

        Task<IReadOnlyList<Play>> GetUserTopPlaysAsync(string userId, GameMode gameMode);

        Task<IReadOnlyList<Play>> GetBeatmapTopPlaysAsync(string beatmapId, GameMode gameMode, Mods mods);
    }
}