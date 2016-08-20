﻿// ------------------------------------------------------------------ 
//  Solution: <OsuHelper>
//  Project: <OsuHelper>
//  File: <Beatmap.cs>
//  Created By: Alexey Golub
//  Date: 20/08/2016
// ------------------------------------------------------------------ 

using System;
using Newtonsoft.Json;
using OsuHelper.Models.Converters;

// ReSharper disable UnusedAutoPropertyAccessor.Local (serialization)

namespace OsuHelper.Models
{
    public class Beatmap
    {
        [JsonProperty("beatmap_id")]
        public string ID { get; private set; }

        public BeatmapRankingStatus Status { get; private set; }

        public string Creator { get; private set; }

        public string Artist { get; private set; }
        public string Title { get; private set; }
        [JsonProperty("version")]
        public string DifficultyName { get; private set; }

        public string FullName => $"{Artist} - {Title} [{DifficultyName}]";

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

        [JsonConverter(typeof(SecondsToTimespanConverter))]
        public TimeSpan TotalLength { get; private set; }
        [JsonConverter(typeof(SecondsToTimespanConverter))]
        public TimeSpan HitLength { get; private set; }

        private Beatmap() { }
    }
}