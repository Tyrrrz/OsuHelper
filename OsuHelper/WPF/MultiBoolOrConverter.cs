// ------------------------------------------------------------------ 
//  Solution: <OsuHelper>
//  Project: <OsuHelper>
//  File: <MultiBoolOrConverter.cs>
//  Created By: Alexey Golub
//  Date: 21/08/2016
// ------------------------------------------------------------------ 

using System;
using System.Linq;
using System.Windows.Data;

namespace OsuHelper.WPF
{
    public class MultiBoolOrConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            return values.Cast<bool>().Any(v => v);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}