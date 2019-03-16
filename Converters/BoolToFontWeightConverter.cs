using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Sungaila.SUBSTitute.Converters
{
    [ValueConversion(typeof(bool), typeof(FontWeight), ParameterType = typeof(FontWeight))]
    public class BoolToFontWeightConverter
        : IValueConverter
    {
        public static readonly BoolToFontWeightConverter Instance = new BoolToFontWeightConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                return Binding.DoNothing;

            FontWeight fontWeight = parameter is FontWeight
                ? (FontWeight)parameter
                : FontWeights.Bold;

            return ((bool)value) ? fontWeight : FontWeights.Normal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
