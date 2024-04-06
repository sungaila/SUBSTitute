using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace Sungaila.SUBSTitute.Converters
{
    public class EnumToStringConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is not Enum enumValue)
                return DependencyProperty.UnsetValue;
            try
            {
                return App.ResourceLoader.GetString($"{enumValue.GetType().Name}+{Enum.GetName(enumValue.GetType(), enumValue)}");
            }
            catch {
                return Enum.GetName(enumValue.GetType(), enumValue);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}