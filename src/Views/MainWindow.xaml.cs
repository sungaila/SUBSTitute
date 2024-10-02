using Microsoft.UI;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Sungaila.ImmersiveDarkMode.WinUI;
using Sungaila.SUBSTitute.ViewModels;
using System;
using System.Linq;
using Windows.Graphics;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
using WinUIEx;
using Icon = System.Drawing.Icon;

namespace Sungaila.SUBSTitute.Views
{
    public sealed partial class MainWindow : WindowEx
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.InitTitlebarTheme();

            var exePath = Environment.ProcessPath!;
            var icon = Icon.ExtractAssociatedIcon(exePath)!;
            this.AppWindow.SetIcon(Win32Interop.GetIconIdFromIcon(icon.Handle));

            if (Content is FrameworkElement frameworkElement)
            {
                frameworkElement.ActualThemeChanged += (_, _) => this.SetTitlebarTheme();
                if (Enum.TryParse<ElementTheme>(App.LocalSettings.Values[nameof(App.RequestedAppTheme)]?.ToString(), out var theme))
                {
                    frameworkElement.RequestedTheme = theme;
                }

                var viewModel = new MainViewModel();
                viewModel.Mapping.QueryDrives.Execute(viewModel.Mapping);
                frameworkElement.DataContext = viewModel;
            }

            if (App.IsElevated)
            {
                this.Title += $" ({App.ResourceLoader.GetString("Administrator")})";
            }

            if (App.LocalSettings.Values["MainWindowWidth"] is double width && App.LocalSettings.Values["MainWindowHeight"] is double height)
            {
                try
                {
                    this.SetWindowSize(width, height);
                }
                catch { }
            }

            if (App.LocalSettings.Values["MainWindowX"] is int x && App.LocalSettings.Values["MainWindowY"] is int y)
            {
                try
                {
                    this.Move(x, y);
                }
                catch { }
            }

            if (App.LocalSettings.Values["MainWindowWindowState"] is int savedWindowState)
            {
                var windowState = (WindowState)savedWindowState;

                if (windowState == WindowState.Maximized)
                    this.WindowState = WindowState.Maximized;
            }
        }

        private void NavigationView_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is not NavigationView navigationView)
                return;

            this.DispatcherQueue.TryEnqueue(DispatcherQueuePriority.Low, () =>
            {
                navigationView.SelectedItem = navigationView.MenuItems.First();
            });
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            Type? pageType = null;

            if (args.IsSettingsSelected)
            {
                pageType = typeof(SettingsView);
            }
            else if (args.SelectedItemContainer?.Tag is Type type)
            {
                pageType = type;
            }

            ContentFrame.Navigate(
                pageType,
                null,
                args.RecommendedNavigationTransitionInfo);
        }

        private void NavigationView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            if (ContentFrame.CanGoBack)
                ContentFrame.GoBack();
        }

        public void ShowInfoBar(string message, InfoBarSeverity severity)
        {
            InfoBar.Severity = severity;
            InfoBar.Message = message;
            InfoBar.IsOpen = true;
        }

#pragma warning disable CA1822
        private void WindowEx_WindowStateChanged(object sender, WindowState e)
#pragma warning restore CA1822
        {
            if (e == WindowState.Minimized)
                return;

            App.LocalSettings.Values["MainWindowWindowState"] = (int)e;
        }

        private void WindowEx_SizeChanged(object sender, WindowSizeChangedEventArgs args)
        {
            if (!this.Visible || this.WindowState != WindowState.Normal)
                return;

            App.LocalSettings.Values["MainWindowWidth"] = args.Size.Width;
            App.LocalSettings.Values["MainWindowHeight"] = args.Size.Height;
        }

        private void WindowEx_PositionChanged(object sender, PointInt32 e)
        {
            if (!this.Visible || this.WindowState != WindowState.Normal)
                return;

            // WindowEx_PositionChanged is called before WindowEx_WindowStateChanged
            // so we have to call Win32 APIs to check the current window state
            WINDOWPLACEMENT windowplacement = new();
            PInvoke.GetWindowPlacement((HWND)this.GetWindowHandle(), ref windowplacement);

            if (windowplacement.showCmd == SHOW_WINDOW_CMD.SW_SHOWMAXIMIZED)
                return;

            App.LocalSettings.Values["MainWindowX"] = e.X;
            App.LocalSettings.Values["MainWindowY"] = e.Y;
        }
    }
}