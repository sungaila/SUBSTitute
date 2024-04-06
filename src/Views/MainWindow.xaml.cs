using Microsoft.UI;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Sungaila.ImmersiveDarkMode.WinUI;
using Sungaila.SUBSTitute.ViewModels;
using System;
using System.Linq;
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

            this.AppWindow.Resize(new(1024, 768));

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
            else if (args.SelectedItemContainer?.Tag is string typeName)
            {
                pageType = Type.GetType(typeName);
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
    }
}