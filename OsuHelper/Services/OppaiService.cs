// ------------------------------------------------------------------ 
//  Solution: <OsuHelper>
//  Project: <OsuHelper>
//  File: <OppaiService.cs>
//  Created By: Alexey Golub
//  Date: 22/08/2016
// ------------------------------------------------------------------ 

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using NegativeLayer.Extensions;
using Newtonsoft.Json;
using OsuHelper.Models.API;

namespace OsuHelper.Services
{
    public sealed class OppaiService
    {
        private static string OppaiExePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "oppai.exe");

        private static async Task<string> RunCmdAsync(string target, params string[] args)
        {
            string argsStr = args.JoinToString(" ");

            string outputBuffer = string.Empty;

            var process = new Process
            {
                StartInfo =
                {
                    FileName = target,
                    Arguments = argsStr,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    UseShellExecute = false
                },
                EnableRaisingEvents = true
            };
            process.OutputDataReceived += (sender, eventArgs) => outputBuffer += eventArgs.Data;
            process.Start();
            process.BeginOutputReadLine();

            await Task.Run(() => process.WaitForExit());

            return outputBuffer;
        }

        public async Task<double> CalculatePerformancePointsAsync(string beatmapFilePath, double accuracy, EnabledMods mods)
        {
            // Compose command line arguments
            var args = new List<string>
            {
                beatmapFilePath,
                100.0*accuracy + "%",
                "-ojson"
            };

            // Mods
            if (!mods.IsEither(EnabledMods.Any, EnabledMods.None))
                args.Add("+" + mods.GetModsString());

            // Run oppai
            string json = await RunCmdAsync(OppaiExePath, args.ToArray());

            // Deserialize
            dynamic result = JsonConvert.DeserializeObject(json);

            // Return pp value
            return result.pp;
        }
    }
}