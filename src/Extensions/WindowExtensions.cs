using Microsoft.UI.Xaml;
using Sungaila.ImmersiveDarkMode.WinUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;

namespace Sungaila.SUBSTitute.Extensions
{
    public static class WindowExtensions
    {
        public static void InitTitlebarTheme2(this Window window)
        {
            ArgumentNullException.ThrowIfNull(window);

            void colorValuesChangedHandler(UISettings sender, object args) => window.SetTitlebarTheme();

            var uiSettings = new UISettings();
            uiSettings.ColorValuesChanged += colorValuesChangedHandler;

            void closedHandler(object sender, WindowEventArgs args)
            {
                window.Closed -= closedHandler;
                uiSettings!.ColorValuesChanged -= colorValuesChangedHandler;
                uiSettings = null;
            };

            window.Closed -= closedHandler;
            window.Closed += closedHandler;

            window.SetTitlebarTheme();
        }
    }
}
