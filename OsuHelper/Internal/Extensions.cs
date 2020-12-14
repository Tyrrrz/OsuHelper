using System;

namespace OsuHelper.Internal
{
    internal static class Extensions
    {
        public static DateTimeOffset ToDateTimeOffset(this DateTime dateTime) => new(dateTime);
    }
}