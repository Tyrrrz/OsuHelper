using OsuHelper.Models;

namespace OsuHelper.Services
{
    public interface IBeatmapProcessorService
    {
        BeatmapTraits CalculateBeatmapTraitsWithMods(Beatmap beatmap, Mods mods);
    }
}