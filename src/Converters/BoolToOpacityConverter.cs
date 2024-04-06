using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace Sungaila.SUBSTitute.Converters
{
    public class BoolToOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is not bool boolean)
                return DependencyProperty.UnsetValue;

            return boolean ? 1.0d : 0.0d;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}