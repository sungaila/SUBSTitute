using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Sungaila.SUBSTitute.ViewModels;

namespace Sungaila.SUBSTitute.Views
{
    public sealed partial class RemoveDriveView : Page
    {
        RemoveDriveViewModel? Data => DataContext as RemoveDriveViewModel;

        public RemoveDriveView()
        {
            this.InitializeComponent();
            AdminIcon.Visibility = !App.IsElevated ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}