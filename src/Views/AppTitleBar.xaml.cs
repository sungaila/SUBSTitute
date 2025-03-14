using Microsoft.UI.Xaml.Controls;

namespace Sungaila.SUBSTitute.Views
{
    public sealed partial class AppTitleBar : TitleBar
    {
        public AppTitleBar()
        {
            this.InitializeComponent();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822", Justification = "Referenced in XAML")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0079:Unnötige Unterdrückung entfernen", Justification = "<Ausstehend>")]
        private void TitleBar_PaneToggleRequested(TitleBar sender, object args)
        {
            if (App.MainWindow == null)
                return;

            App.MainWindow.NavigationView.IsPaneOpen = !App.MainWindow.NavigationView.IsPaneOpen;
        }
    }
}