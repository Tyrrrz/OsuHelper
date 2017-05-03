using System;
using System.Globalization;
using System.Windows.Data;
using OsuHelper.Models;

namespace OsuHelper.Converters
{
    [ValueConversion(typeof(Mods), typeof(string))]
    public class EnabledModsToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            var mods = (Mods) value;
            return mods.FormatMods();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}