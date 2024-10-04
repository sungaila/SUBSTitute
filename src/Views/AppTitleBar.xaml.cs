using CommunityToolkit.WinUI.Controls;
using Microsoft.UI.Xaml;

namespace Sungaila.SUBSTitute.Views
{
    public sealed partial class AppTitleBar : TitleBar
    {
        private MainWindow MainWindow => App.MainWindow!;

        public AppTitleBar()
        {
            this.InitializeComponent();
        }

        private void AppTitleBar_PaneButtonClick(object sender, RoutedEventArgs args)
        {
            MainWindow.NavigationView.IsPaneOpen = !MainWindow.NavigationView.IsPaneOpen;
        }
    }
}
