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
    public class OppaiBeatmapProcessorService : IBeatmapProcessorService, IDisposable
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
                argsBuffer.Append(mods.FormatToString());
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
            var beatmapRaw = await _dataService.GetBeatmapRawAsync(beatmap.Id);

            // Run oppai
            var oppaiOutput = await ExecuteOppaiAsync(beatmapRaw, mods);

            // Parse
            var oppaiOutputJson = JToken.Parse(oppaiOutput);

            // Populate result
            var maxCombo = beatmap.Traits.MaxCombo;
            var duration = beatmap.Traits.Duration;
            var bpm = beatmap.Traits.BeatsPerMinute;
            if (mods.HasFlag(Mods.DoubleTime))
            {
                duration = TimeSpan.FromSeconds(beatmap.Traits.Duration.TotalSeconds / 1.5);
                bpm = beatmap.Traits.BeatsPerMinute * 1.5;
            }
            else if (mods.HasFlag(Mods.HalfTime))
            {
                duration = TimeSpan.FromSeconds(beatmap.Traits.Duration.TotalSeconds / 0.75);
                bpm = beatmap.Traits.BeatsPerMinute * 0.75;
            }
            var sr = oppaiOutputJson["stars"].Value<double>();
            var ar = oppaiOutputJson["ar"].Value<double>();
            var od = oppaiOutputJson["od"].Value<double>();
            var cs = oppaiOutputJson["cs"].Value<double>();
            var hp = oppaiOutputJson["hp"].Value<double>();

            return new BeatmapTraits(maxCombo, duration, bpm, sr, ar, od, cs, hp);
        }

        public void Dispose()
        {
            _cli.CancelAll();
            _cli.Dispose();
        }
    }
}