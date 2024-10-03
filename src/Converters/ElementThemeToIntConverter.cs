using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace Sungaila.SUBSTitute.Converters
{
    public partial class ElementThemeToIntConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is not ElementTheme theme)
                return DependencyProperty.UnsetValue;

            return theme switch
            {
                ElementTheme.Light => 0,
                ElementTheme.Dark => 1,
                _ => 2
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is not int selectedIndex)
                return DependencyProperty.UnsetValue;

            return selectedIndex switch
            {
                0 => ElementTheme.Light,
                1 => ElementTheme.Dark,
                _ => ElementTheme.Default
            };
        }
    }
}