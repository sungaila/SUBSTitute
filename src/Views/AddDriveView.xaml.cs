using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Sungaila.SUBSTitute.ViewModels;
using System;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace Sungaila.SUBSTitute.Views
{
    public sealed partial class AddDriveView : Page
    {
        private AddDriveViewModel? Data => DataContext as AddDriveViewModel;

        public AddDriveView()
        {
            this.InitializeComponent();
            AdminIcon.Visibility = !App.IsElevated ? Visibility.Visible : Visibility.Collapsed;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var folderPicker = new FolderPicker();
            var hWnd = WindowNative.GetWindowHandle(App.MainWindow);
            InitializeWithWindow.Initialize(folderPicker, hWnd);
            folderPicker.SuggestedStartLocation = PickerLocationId.ComputerFolder;

            if (await folderPicker.PickSingleFolderAsync() is not StorageFolder folder)
                return;

            StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", folder);
            PathTextBox.Text = folder.Path;
        }
    }
}