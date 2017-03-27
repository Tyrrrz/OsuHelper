using System.Collections.Generic;
using System.Threading.Tasks;
using OsuHelper.Models;

namespace OsuHelper.Services
{
    public interface IDataService
    {
        string ApiKey { get; set; }

        Task<Beatmap> GetBeatmapAsync(GameMode gameMode, string beatmapId);

        Task<IEnumerable<Play>> GetUserTopPlaysAsync(GameMode gameMode, string userId);

        Task<IEnumerable<Play>> GetBeatmapTopPlaysAsync(GameMode gameMode, string beatmapId, EnabledMods enabledMods);
    }
}