using System.Text;
using OsuHelper.Models;

namespace OsuHelper
{
    public static class Extensions
    {
        public static string FormatMods(this Mods mods)
        {
            if (mods == Mods.None) return string.Empty;

            var buffer = new StringBuilder();

            // Only mods that influence PP and/or are ranked
            if (mods.HasFlag(Mods.NoFail))
                buffer.Append("NF");
            if (mods.HasFlag(Mods.Easy))
                buffer.Append("EZ");
            if (mods.HasFlag(Mods.Hidden))
                buffer.Append("HD");
            if (mods.HasFlag(Mods.HardRock))
                buffer.Append("HR");
            if (mods.HasFlag(Mods.DoubleTime) || mods.HasFlag(Mods.Nightcore))
                buffer.Append("DT");
            if (mods.HasFlag(Mods.HalfTime))
                buffer.Append("HT");
            if (mods.HasFlag(Mods.Flashlight))
                buffer.Append("FL");
            if (mods.HasFlag(Mods.SpunOut))
                buffer.Append("SO");

            return buffer.ToString();
        }
    }
}