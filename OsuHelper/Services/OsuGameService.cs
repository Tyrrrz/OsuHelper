// -------------------------------------------------------------------------
// Solution: OsuHelper
// Project: OsuHelper
// File: OsuGameService.cs
// 
// Created by: Tyrrrz
// On: 28.08.2016
// -------------------------------------------------------------------------

using System.Diagnostics;
using System.Linq;
using NegativeLayer.Extensions;

namespace OsuHelper.Services
{
    public sealed class OsuGameService
    {
        public string GetNowPlayingTitle()
        {
            // Find osu process
            var process = Process.GetProcessesByName("osu!").FirstOrDefault();
            if (process == null) return null;

            // Get title
            string windowTitle = process.MainWindowTitle;

            // Check if anything is playing
            if (!windowTitle.StartsWith("osu!  - "))
                return null;

            // Return the title
            return windowTitle.SubstringAfter("osu!  - ");
        }
    }
}
