using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using System;

namespace Sungaila.SUBSTitute.Converters
{
    public partial class NavigationViewDisplayModeToThicknessConverter : IValueConverter
    {
        public Thickness Default { get; set; }

        public Thickness Minimal { get; set; }

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

            return result ? Default : Minimal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}