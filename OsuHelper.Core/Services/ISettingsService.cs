using System.Collections.Generic;
using OsuHelper.Models;

namespace OsuHelper.Services
{
    public interface ISettingsService
    {
        IEnumerable<BeatmapRecommendation> LastRecommendations { get; set; }
        string OsuApiRoot { get; set; }
        string OsuApiKey { get; set; }

        void Load();
        void Save();
    }
}