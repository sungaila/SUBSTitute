using CommunityToolkit.WinUI.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Sungaila.SUBSTitute.ViewModels;
using System;
using System.Reflection;
using Windows.ApplicationModel;
using Windows.System;

namespace Sungaila.SUBSTitute.Views
{
    public sealed partial class SettingsView : Page
    {
        private MainViewModel? Data => DataContext as MainViewModel;

        public SettingsView()
        {
            this.InitializeComponent();

            string nameAndVersion;
            string author;

            try
            {
                nameAndVersion = $"{Package.Current.DisplayName} {Package.Current.Id.Version.ToFormattedString()}";
                author = Package.Current.PublisherDisplayName;
            }
            catch
            {
                var assemblyName = typeof(App).Assembly.GetName();
                nameAndVersion = $"{assemblyName.Name} {assemblyName.Version}";
                author = typeof(App).Assembly.GetCustomAttribute<AssemblyCompanyAttribute>()?.Company ?? string.Empty;
            }

            AppNameTextBlock.Text = nameAndVersion;
            AuthorTextBlock.Text = author;
        }

        private void Page_Loading(FrameworkElement sender, object args)
        {
            if (this.DataContext is MainViewModel mainViewModel)
            {
                mainViewModel.Settings.SelectedTheme = App.RequestedAppTheme;
            }
        }

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is ComboBox comboBox)
                comboBox.SelectionChanged += (_, _) => App.MainWindow?.ShowInfoBar(App.ResourceLoader.GetString("SettingsRestartRequired"), InfoBarSeverity.Warning);
        }

        private static readonly Uri _githubIssuesUri = new("https://github.com/sungaila/SUBSTitute/issues");

        private async void SettingsCard_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(_githubIssuesUri);
        }
    }
}