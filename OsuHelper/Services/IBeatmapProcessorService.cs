using OsuHelper.Models;

namespace OsuHelper.Services
{
    public interface IBeatmapProcessorService
    {
        BeatmapTraits CalculateTraitsWithMods(Beatmap beatmap, Mods mods);
    }
}