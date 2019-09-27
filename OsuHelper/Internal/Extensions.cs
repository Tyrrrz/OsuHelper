using System;

namespace OsuHelper.Internal
{
    internal static class Extensions
    {
        public static DateTimeOffset ToDateTimeOffset(this DateTime dateTime) => new DateTimeOffset(dateTime);

        public static void OpenInBrowser(this Uri uri) => ProcessEx.StartShellExecute(uri.ToString());
    }
}