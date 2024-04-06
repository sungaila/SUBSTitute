using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using WinRT.Interop;


namespace Sungaila.SUBSTitute.Views
{
    public sealed partial class AddDriveView : Page
    {
        public AddDriveView()
        {
            this.InitializeComponent();
            AdminIcon.Visibility = !App.IsElevated ? Visibility.Visible : Visibility.Collapsed;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var openPicker = new FolderPicker();
            var hWnd = WindowNative.GetWindowHandle(App.MainWindow);

            InitializeWithWindow.Initialize(openPicker, hWnd);

            var folder = await openPicker.PickSingleFolderAsync();

            if (folder == null)
                return;

            StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", folder);
            PathTextBox.Text = folder.Path;
        }
    }
}