using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Sungaila.SUBSTitute.ViewModels;
using Sungaila.SUBSTitute.Views;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Windows.Input;
using Windows.Win32;
using Windows.Win32.Storage.FileSystem;

namespace Sungaila.SUBSTitute.Commands
{
    public static class DriveCommands
    {
        private static IRelayCommand<DriveViewModel> OpenInternal { get; } = new RelayCommand<DriveViewModel>(parameter =>
        {
            Process.Start(new ProcessStartInfo(parameter!.Path) { UseShellExecute = true });
        });

        public static ICommand Open { get; } = new StandardUICommand(StandardUICommandKind.Open) { Command = OpenInternal, Description = "Open in Explorer" };

        private static IAsyncRelayCommand<DriveViewModel> DisconnectInternal { get; } = new AsyncRelayCommand<DriveViewModel>(async parameter =>
        {
            var dataContext = new RemoveDriveViewModel
            {
                ParentViewModel = parameter!.ParentViewModel
            };

            var dialog = new ContentDialog
            {
                XamlRoot = App.MainWindow?.Content.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                Title = App.ResourceLoader.GetString("RemoveDriveViewTitle"),
                PrimaryButtonText = App.ResourceLoader.GetString("ContentDialogRemove"),
                CloseButtonText = App.ResourceLoader.GetString("ContentDialogCancel"),
                DefaultButton = ContentDialogButton.Primary,
                Content = new RemoveDriveView(),
                UseLayoutRounding = true,
                RequestedTheme = App.RequestedAppTheme,
                DataContext = dataContext
            };

            if (await dialog.ShowAsync() != ContentDialogResult.Primary)
                return;

            RemoveDosDevice(parameter.Letter);
            parameter.ParentViewModel.Drives.Remove(parameter);
        },
        parameter => parameter?.IsVirtual == true);

        private static void RemoveDosDevice(char driveLetter)
        {
            if (!PInvoke.DefineDosDevice(DEFINE_DOS_DEVICE_FLAGS.DDD_REMOVE_DEFINITION, $"{driveLetter}:", null))
                throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        public static ICommand Disconnect { get; } = new StandardUICommand(StandardUICommandKind.Delete) { Command = DisconnectInternal, Description = "Remove this virtual drive" };
    }
}