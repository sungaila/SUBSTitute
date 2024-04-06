using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Sungaila.SUBSTitute.ViewModels;
using Sungaila.SUBSTitute.Views;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;
using Windows.Win32;
using Windows.Win32.Storage.FileSystem;

namespace Sungaila.SUBSTitute.Commands
{
    public static class DriveCommands
    {
        private static readonly IRelayCommand<DriveViewModel> OpenInternal = new RelayCommand<DriveViewModel>(parameter =>
        {
            Process.Start(new ProcessStartInfo(parameter!.Path) { UseShellExecute = true });
        });

        public static readonly ICommand Open = new StandardUICommand(StandardUICommandKind.Open)
        {
            Command = OpenInternal,
            Description = App.ResourceLoader.GetString("Open+Description")
        };

        private static readonly IAsyncRelayCommand<DriveViewModel> RemoveVirtualDriveInternal = new AsyncRelayCommand<DriveViewModel>(async parameter =>
        {
            var dataContext = new RemoveDriveViewModel(parameter!)
            {
                ParentViewModel = parameter!.ParentViewModel
            };

            var dialog = new ContentDialog
            {
                XamlRoot = App.MainWindow?.Content.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                Title = App.ResourceLoader.GetString("RemoveDriveViewTitle"),
                PrimaryButtonText = App.ResourceLoader.GetString("ContentDialogRemove"),
                PrimaryButtonCommand = RemoveDosDeviceInternal,
                PrimaryButtonCommandParameter = dataContext,
                CloseButtonText = App.ResourceLoader.GetString("ContentDialogCancel"),
                DefaultButton = ContentDialogButton.Primary,
                Content = new RemoveDriveView(),
                UseLayoutRounding = true,
                RequestedTheme = App.RequestedAppTheme,
                DataContext = dataContext
            };

            dialog.Closing += (sender, e) =>
            {
                e.Cancel = e.Result == ContentDialogResult.Primary && dataContext.CancelClose;
            };

            dialog.IsPrimaryButtonEnabled = dialog.PrimaryButtonCommand!.CanExecute(dataContext);

            dialog.PrimaryButtonCommand.CanExecuteChanged += (sender, e) =>
            {
                dialog.IsPrimaryButtonEnabled = dialog.PrimaryButtonCommand.CanExecute(dataContext);
            };

            await dialog.ShowAsync();
        },
        parameter => parameter?.IsVirtual == true);

        private static readonly IRelayCommand<RemoveDriveViewModel> RemoveDosDeviceInternal = new RelayCommand<RemoveDriveViewModel>(parameter =>
        {
            parameter!.CancelClose = false;

            if (parameter.RemovePermanent)
            {
                try
                {
                    Process.Start(new ProcessStartInfo(
                        $"reg.exe",
                        $"delete \"HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\DOS Devices\" /v \"{parameter.DriveToRemove.Letter}:\" /f")
                    {
                        UseShellExecute = true,
                        Verb = "runas",
                        WindowStyle = ProcessWindowStyle.Hidden
                    });
                }
                catch (Win32Exception ex) when (ex.NativeErrorCode == 0x4C7)
                {
                    // ERROR_CANCELLED: The operation was canceled by the user.
                    parameter.CancelClose = true;
                    return;
                }
            }

            RemoveDosDevice(parameter.DriveToRemove.Letter);
            parameter.ParentViewModel.Drives.Remove(parameter.DriveToRemove);
        });

        public static readonly ICommand RemoveVirtualDrive = new StandardUICommand(StandardUICommandKind.Delete)
        {
            Command = RemoveVirtualDriveInternal,
            Label = App.ResourceLoader.GetString("RemoveVirtualDrive+Label"),
            Description = App.ResourceLoader.GetString("RemoveVirtualDrive+Description")
        };

        private static void RemoveDosDevice(char driveLetter)
        {
            if (!PInvoke.DefineDosDevice(DEFINE_DOS_DEVICE_FLAGS.DDD_REMOVE_DEFINITION, $"{driveLetter}:", null))
                throw new Win32Exception(Marshal.GetLastWin32Error());
        }
    }
}