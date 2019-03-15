using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Sungaila.SUBSTitute.Converters
{
    [ValueConversion(typeof(bool), typeof(GridLength))]
    public class BoolToGridLengthConverter
        : IValueConverter
    {
        public static readonly BoolToGridLengthConverter Instance = new BoolToGridLengthConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                return Binding.DoNothing;

            return (bool)value ? new GridLength(1.0, GridUnitType.Star) : GridLength.Auto;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
