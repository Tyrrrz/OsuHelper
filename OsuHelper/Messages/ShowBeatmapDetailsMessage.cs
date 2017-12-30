using OsuHelper.Models;

namespace OsuHelper.Messages
{
    public class ShowBeatmapDetailsMessage
    {
        public Beatmap Beatmap { get; }

        public ShowBeatmapDetailsMessage(Beatmap beatmap)
        {
            Beatmap = beatmap;
        }
    }
}