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
            if (value is null)
                return null;

            var time = (TimeSpan) value;
            var fullMins = time.Hours * 60 + time.Minutes;

            return $"{fullMins:00}:{time.Seconds:00}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}