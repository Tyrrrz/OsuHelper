using System;
using System.Globalization;
using System.Windows.Data;
using OsuHelper.Models;

namespace OsuHelper.Converters
{
    [ValueConversion(typeof(Mods), typeof(string))]
    public class ModsToStringConverter : IValueConverter
    {
        public static ModsToStringConverter Instance { get; } = new();

        public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            var mods = (Mods?) value;
            return mods?.FormatToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}