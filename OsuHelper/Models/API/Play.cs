// ------------------------------------------------------------------ 
//  Solution: <OsuHelper>
//  Project: <OsuHelper>
//  File: <Play.cs>
//  Created By: Alexey Golub
//  Date: 20/08/2016
// ------------------------------------------------------------------ 

// ReSharper disable UnusedAutoPropertyAccessor.Local (serialization)

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace OsuHelper.Models.API
{
    public class Play
    {
        [JsonProperty("beatmap_id")]
        public string BeatmapID { get; private set; }

        [JsonProperty("user_id")]
        public string UserID { get; private set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("rank")]
        public PlayRank Rank { get; private set; }

        [JsonProperty("count300")]
        public int Count300 { get; private set; }
        [JsonProperty("count100")]
        public int Count100 { get; private set; }
        [JsonProperty("count50")]
        public int Count50 { get; private set; }
        [JsonProperty("countmiss")]
        public int CountMiss { get; private set; }

        [JsonIgnore]
        public double Accuracy
            => (Count50*50 + Count100*100 + Count300*300)/(300.0*(Count50 + Count100 + Count300 + CountMiss));

        [JsonProperty("maxcombo")]
        public int MaxCombo { get; private set; }

        [JsonProperty("pp")]
        public double PerformancePoints { get; private set; }

        [JsonProperty("enabled_mods")]
        public EnabledMods Mods { get; private set; }

        private Play() { }
    }
}