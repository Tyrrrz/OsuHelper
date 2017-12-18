using System.Threading.Tasks;
using OsuHelper.Models;

namespace OsuHelper.Services
{
    public interface IBeatmapProcessorService
    {
        Task<BeatmapTraits> CalculateTraitsWithModsAsync(Beatmap beatmap, Mods mods);
    }
}