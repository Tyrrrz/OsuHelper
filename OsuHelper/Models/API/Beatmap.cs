// ------------------------------------------------------------------ 
//  Solution: <OsuHelper>
//  Project: <OsuHelper>
//  File: <Beatmap.cs>
//  Created By: Alexey Golub
//  Date: 20/08/2016
// ------------------------------------------------------------------ 

// ReSharper disable UnusedAutoPropertyAccessor.Local (serialization)

using System;
using NegativeLayer.Extensions;
using Newtonsoft.Json;
using OsuHelper.Models.Converters;

namespace OsuHelper.Models.API
{
    public class Beatmap
    {
        [JsonProperty("beatmap_id")]
        public string ID { get; private set; }
        [JsonProperty("beatmapset_id")]
        public string MapSetID { get; private set; }
        [JsonProperty("mode")]
        public GameMode Mode { get; private set; }

        [JsonProperty("approved")]
        public BeatmapRankingStatus Status { get; private set; }

        [JsonIgnore]
        public string ThumbnailURL => $"https://b.ppy.sh/thumb/{MapSetID}l.jpg";

        [JsonProperty("creator")]
        public string Creator { get; private set; }

        [JsonProperty("last_update")]
        public DateTime LastUpdateDate { get; private set; }

        [JsonProperty("artist")]
        public string Artist { get; private set; }
        [JsonProperty("title")]
        public string Title { get; private set; }
        [JsonProperty("version")]
        public string DifficultyName { get; private set; }

        [JsonIgnore]
        public string FullName => $"{Artist} - {Title} [{DifficultyName}]";

        [JsonConverter(typeof(SecondsToTimespanConverter))]
        [JsonProperty("total_length")]
        public TimeSpan TotalLength { get; private set; }
        [JsonConverter(typeof(SecondsToTimespanConverter))]
        [JsonProperty("hit_length")]
        public TimeSpan HitLength { get; private set; }
        [JsonProperty("max_combo", NullValueHandling = NullValueHandling.Ignore)] // this is set to null on some game modes
        public int MaxCombo { get; private set; }

        [JsonIgnore]
        public string TotalLengthString => $"{Math.Truncate(TotalLength.TotalMinutes):00}:{TotalLength.Seconds:00}";
        [JsonIgnore]
        public string HitLengthString => $"{Math.Truncate(HitLength.TotalMinutes):00}:{HitLength.Seconds:00}";

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

        [JsonIgnore]
        public double ApproachRateHardRock => (ApproachRate*1.4).ClampMax(10);
        [JsonIgnore]
        public double OverallDifficultyHardRock => (OverallDificulty*1.4).ClampMax(10);

        private Beatmap() { }
    }
}