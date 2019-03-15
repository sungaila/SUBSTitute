using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Sungaila.SUBSTitute.Converters
{
    public class EqualityToBoolConverter
        : IMultiValueConverter
    {
        public static readonly EqualityToBoolConverter Instance = new EqualityToBoolConverter();

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 0)
                return Binding.DoNothing;

            object previousValue = values[0];

            for (int i = 1; i < values.Length; i++)
            {
                if (previousValue != values[i])
                    return false;
            }

            return true;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
