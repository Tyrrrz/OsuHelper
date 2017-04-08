using System;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using OsuHelper.Models;

namespace OsuHelper.Converters
{
    [ValueConversion(typeof(EnabledMods), typeof(string))]
    public class EnabledModsToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            var mods = (EnabledMods) value;

            if (mods == EnabledMods.None) return string.Empty;

            var buffer = new StringBuilder();

            // Only mods that influence PP and/or are ranked
            if (mods.HasFlag(EnabledMods.NoFail))
                buffer.Append("NF");
            if (mods.HasFlag(EnabledMods.Easy))
                buffer.Append("EZ");
            if (mods.HasFlag(EnabledMods.Hidden))
                buffer.Append("HD");
            if (mods.HasFlag(EnabledMods.HardRock))
                buffer.Append("HR");
            if (mods.HasFlag(EnabledMods.DoubleTime) || mods.HasFlag(EnabledMods.Nightcore))
                buffer.Append("DT");
            if (mods.HasFlag(EnabledMods.HalfTime))
                buffer.Append("HT");
            if (mods.HasFlag(EnabledMods.Flashlight))
                buffer.Append("FL");
            if (mods.HasFlag(EnabledMods.SpunOut))
                buffer.Append("SO");

            return buffer.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}