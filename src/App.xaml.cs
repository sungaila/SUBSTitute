using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Windows.ApplicationModel.Resources;
using Sungaila.SUBSTitute.Views;
using System;
using System.Security.Principal;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Sungaila.SUBSTitute
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        public static readonly ResourceLoader ResourceLoader = new();

        public static readonly ApplicationDataContainer LocalSettings = ApplicationData.Current.LocalSettings;

        public static MainWindow? MainWindow { get; private set; }

        public static bool IsElevated { get; private set; }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.UnhandledException += Application_UnhandledException;

            using var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            IsElevated = principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            MainWindow = new MainWindow();
            MainWindow.Activate();
        }

        private void Application_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            if (MainWindow == null)
                return;

            MainWindow.ShowInfoBar(e.Message, InfoBarSeverity.Error);
            e.Handled = true;
        }

        public static ElementTheme RequestedAppTheme
        {
            get => (MainWindow?.Content as FrameworkElement)?.RequestedTheme ?? ElementTheme.Default;
            set
            {
                if (MainWindow?.Content is FrameworkElement root)
                {
                    root.RequestedTheme = value;
                    LocalSettings.Values[nameof(RequestedAppTheme)] = Enum.GetName(value);
                }   
            }
        }
    }
}