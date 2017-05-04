using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CliWrap;
using CliWrap.Models;
using Newtonsoft.Json.Linq;
using OsuHelper.Models;

namespace OsuHelper.Services
{
    public class OppaiBeatmapProcessorService : IBeatmapProcessorService
    {
        private readonly IDataService _dataService;

        private readonly Cli _cli;

        public OppaiBeatmapProcessorService(IDataService dataService)
        {
            _dataService = dataService;
            _cli = new Cli(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "External\\", "oppai.exe"));
        }

        private async Task<string> ExecuteOppaiAsync(string rawBeatmapData, Mods mods)
        {
            // Assemble arguments
            var argsBuffer = new StringBuilder();

            // -- beatmap
            argsBuffer.Append('-');
            argsBuffer.Append(' ');

            // -- mods
            if (mods != Mods.None)
            {
                argsBuffer.Append('+');
                argsBuffer.Append(mods.FormatMods());
                argsBuffer.Append(' ');
            }

            // -- output type
            argsBuffer.Append("-ojson");

            // Execute
            var input = new ExecutionInput(argsBuffer.ToString(), rawBeatmapData);
            var output = await _cli.ExecuteAsync(input);
            output.ThrowIfError();

            return output.StandardOutput;
        }

        public async Task<BeatmapTraits> CalculateTraitsWithModsAsync(Beatmap beatmap, Mods mods)
        {
            // No mods - just return base traits
            if (mods == Mods.None)
                return beatmap.Traits;

            // Not standard - return base traits (oppai doesn't support other modes)
            if (beatmap.GameMode != GameMode.Standard)
                return beatmap.Traits;

            // Get raw beatmap data
            string beatmapRaw = await _dataService.GetBeatmapRawAsync(beatmap.Id);

            // Run oppai
            string oppaiOutput = await ExecuteOppaiAsync(beatmapRaw, mods);

            // Parse
            var parsed = JToken.Parse(oppaiOutput);

            // Populate result
            var result = new BeatmapTraits();
            result.MaxCombo = beatmap.Traits.MaxCombo;
            if (mods.HasFlag(Mods.DoubleTime))
            {
                result.Duration = TimeSpan.FromSeconds(beatmap.Traits.Duration.TotalSeconds / 1.5);
                result.BeatsPerMinute = beatmap.Traits.BeatsPerMinute * 1.5;
            }
            else if (mods.HasFlag(Mods.HalfTime))
            {
                result.Duration = TimeSpan.FromSeconds(beatmap.Traits.Duration.TotalSeconds / 0.75);
                result.BeatsPerMinute = beatmap.Traits.BeatsPerMinute * 0.75;
            }
            else
            {
                result.Duration = beatmap.Traits.Duration;
                result.BeatsPerMinute = beatmap.Traits.BeatsPerMinute;
            }
            result.StarRating = parsed["stars"].Value<double>();
            result.ApproachRate = parsed["ar"].Value<double>();
            result.OverallDifficulty = parsed["od"].Value<double>();
            result.CircleSize = parsed["cs"].Value<double>();
            result.Drain = parsed["hp"].Value<double>();

            return result;
        }
    }
}