using System;
using System.Diagnostics;

namespace OsuHelper.Internal
{
    internal static class Extensions
    {
        public static DateTimeOffset ToDateTimeOffset(this DateTime dateTime) => new DateTimeOffset(dateTime);

        public static void OpenInBrowser(this Uri uri)
        {
            var startInfo = new ProcessStartInfo(uri.ToString())
            {
                UseShellExecute = true
            };

            using (Process.Start(startInfo))
            { }
        }
    }
}