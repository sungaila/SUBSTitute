using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Sungaila.SUBSTitute.Converters
{
    [ValueConversion(typeof(object), typeof(bool))]
    public class NullToReversedBoolConverter
        : IValueConverter
    {
        public static readonly NullToReversedBoolConverter Instance = new NullToReversedBoolConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
