// ------------------------------------------------------------------ 
//  Solution: <OsuHelper>
//  Project: <OsuHelper>
//  File: <Beatmap.cs>
//  Created By: Alexey Golub
//  Date: 20/08/2016
// ------------------------------------------------------------------ 

// ReSharper disable UnusedAutoPropertyAccessor.Local (serialization)

using System;
using Newtonsoft.Json;
using OsuHelper.Models.Converters;

namespace OsuHelper.Models.API
{
    public class Beatmap
    {
        [JsonProperty("beatmap_id")]
        public string ID { get; private set; }

        [JsonProperty("approved")]
        public BeatmapRankingStatus Status { get; private set; }

        public string ThumbnailURL => $"https://b.ppy.sh/thumb/{ID}l.jpg";

        [JsonProperty("creator")]
        public string Creator { get; private set; }

        [JsonProperty("artist")]
        public string Artist { get; private set; }
        [JsonProperty("title")]
        public string Title { get; private set; }
        [JsonProperty("version")]
        public string DifficultyName { get; private set; }

        public string FullName => $"{Artist} - {Title} [{DifficultyName}]";

        [JsonConverter(typeof(SecondsToTimespanConverter))]
        [JsonProperty("total_length")]
        public TimeSpan TotalLength { get; private set; }
        [JsonConverter(typeof(SecondsToTimespanConverter))]
        [JsonProperty("hit_length")]
        public TimeSpan HitLength { get; private set; }
        [JsonProperty("max_combo")]
        public int MaxCombo { get; private set; }

        [JsonProperty("bpm")]
        public double BeatsPerMinute { get; private set; }
        [JsonProperty("difficultyrating")]
        public double Stars { get; private set; }
        [JsonProperty("diff_size")]
        public double CircleSize { get; private set; }
        [JsonProperty("diff_overall")]
        public double OverallDificulty { get; private set; }
        [JsonProperty("diff_approach")]
        public double ApproachRate { get; private set; }
        [JsonProperty("diff_drain")]
        public double Drain { get; private set; }

        private Beatmap() { }
    }
}