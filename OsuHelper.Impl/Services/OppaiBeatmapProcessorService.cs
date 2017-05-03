using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CliWrap;
using Newtonsoft.Json.Linq;
using OsuHelper.Models;

namespace OsuHelper.Services
{
    public class OppaiBeatmapProcessorService : IBeatmapProcessorService
    {
        private readonly IHttpService _httpService;

        private readonly Cli _cli;

        public OppaiBeatmapProcessorService(IHttpService httpService)
        {
            _httpService = httpService;
            _cli = new Cli(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "oppai.exe"));
        }

        private async Task<string> ExecuteOppaiAsync(string beatmapFilePath, Mods mods)
        {
            // Assemble arguments
            var argsBuffer = new StringBuilder();

            // -- beatmap
            argsBuffer.Append('"');
            argsBuffer.Append(beatmapFilePath);
            argsBuffer.Append('"');
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
            var output = await _cli.ExecuteAsync(argsBuffer.ToString());
            output.ThrowIfError();

            return output.StandardOutput;
        }

        public async Task<BeatmapTraits> CalculateTraitsWithModsAsync(Beatmap beatmap, Mods mods)
        {
            // Download map
            string url = $"https://osu.ppy.sh/osu/{beatmap.Id}";
            string beatmapFilePath = Path.GetTempFileName();
            await _httpService.DownloadAsync(url, beatmapFilePath);

            // Run oppai
            string oppaiOutput = await ExecuteOppaiAsync(beatmapFilePath, mods);

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