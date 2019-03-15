using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Sungaila.SUBSTitute.Converters
{
    [ValueConversion(typeof(object), typeof(Visibility), ParameterType=typeof(bool))]
    public class NullToVisibilityConverter
        : IValueConverter
    {
        public static readonly NullToVisibilityConverter Instance = new NullToVisibilityConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool? hiddenParameter = parameter as bool?;

            if (hiddenParameter.HasValue && hiddenParameter.Value)
                return value != null ? Visibility.Visible : Visibility.Hidden;

            return value != null ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
