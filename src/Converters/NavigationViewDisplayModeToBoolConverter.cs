using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using System;

namespace Sungaila.SUBSTitute.Converters
{
    public partial class NavigationViewDisplayModeToBoolConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is not NavigationViewDisplayMode displayMode)
                return DependencyProperty.UnsetValue;

            var result = displayMode != NavigationViewDisplayMode.Minimal;

            // Negate if needed
            if (bool.TryParse(parameter?.ToString(), out var negate) && negate)
            {
                result = !result;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}