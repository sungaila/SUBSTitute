using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace Sungaila.SUBSTitute.Core
{
    public static class ButtonExtensions
    {
        public static readonly DependencyProperty ViewModelCommandProperty = DependencyProperty.RegisterAttached(
            "ViewModelCommand",
            typeof(IViewModelCommand),
            typeof(ButtonExtensions),
            new FrameworkPropertyMetadata(null, ViewModelCommandPropertyChanged));

        private static void ViewModelCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Button? button = d as Button;

            if (button == null)
                return;

            Binding binding = BindingOperations.GetBinding(button, ViewModelCommandProperty);

            BindingOperations.SetBinding(button, ButtonBase.CommandProperty, binding);
            BindingOperations.SetBinding(button, ButtonBase.CommandParameterProperty, binding != null ? new Binding() : null);

            GetViewModelCommand(button)?.RaiseCanExecuteChanged();
        }

        public static IViewModelCommand GetViewModelCommand(Button button)
        {
            return (IViewModelCommand)button.GetValue(ViewModelCommandProperty);
        }

        public static void SetViewModelCommand(Button button, IViewModelCommand value)
        {
            button.SetValue(ViewModelCommandProperty, value);
        }
    }
}
