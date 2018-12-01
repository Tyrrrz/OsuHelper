using System;
using System.Globalization;
using System.Windows.Data;

namespace OsuHelper.Converters
{
    [ValueConversion(typeof(TimeSpan), typeof(string))]
    public class TimeSpanToShortStringConverter : IValueConverter
    {
        public static TimeSpanToShortStringConverter Instance { get; } = new TimeSpanToShortStringConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            var ts = (TimeSpan) value;
            var fullMins = ts.Hours*60 + ts.Minutes;

            return $"{fullMins:00}:{ts.Seconds:00}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}