using System;
using System.Globalization;
using System.Windows.Data;
using OsuHelper.Models;

namespace OsuHelper.Converters
{
    [ValueConversion(typeof(Mods), typeof(string))]
    public class ModsToStringConverter : IValueConverter
    {
        public static ModsToStringConverter Instance { get; } = new ModsToStringConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
                return null;

            var mods = (Mods) value;

            return mods.FormatToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}