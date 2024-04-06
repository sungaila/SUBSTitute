using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Sungaila.SUBSTitute.Views
{
    public sealed partial class RemoveDriveView : Page
    {
        public RemoveDriveView()
        {
            this.InitializeComponent();
            AdminIcon.Visibility = !App.IsElevated ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}