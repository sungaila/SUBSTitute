using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Windows.ApplicationModel.Resources;
using Sungaila.SUBSTitute.Views;
using System;
using System.Linq;
using System.Security.Principal;
using Windows.Globalization;
using Windows.Storage;


namespace Sungaila.SUBSTitute
{
    public partial class App : Application
    {
        public static readonly ResourceLoader ResourceLoader = new();

        public static readonly ApplicationDataContainer LocalSettings = ApplicationData.Current.LocalSettings;

        public static MainWindow? MainWindow { get; private set; }

        public static bool IsElevated { get; private set; }

        public App()
        {
            this.InitializeComponent();
            this.UnhandledException += Application_UnhandledException;

            using var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            IsElevated = principal.IsInRole(WindowsBuiltInRole.Administrator);

            HandlePrimaryLanguageOverride();
        }

        private static void HandlePrimaryLanguageOverride()
        {
            if (!string.IsNullOrEmpty(ApplicationLanguages.PrimaryLanguageOverride))
                return;

            var supportedLanguage = ApplicationLanguages.Languages.FirstOrDefault(l => ApplicationLanguages.ManifestLanguages.Contains(l));

            ApplicationLanguages.PrimaryLanguageOverride = supportedLanguage ?? "en-US";
        }

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