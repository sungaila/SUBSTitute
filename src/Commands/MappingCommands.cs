using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Sungaila.SUBSTitute.ViewModels;
using Sungaila.SUBSTitute.Views;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Win32;

namespace Sungaila.SUBSTitute.Commands
{
    public static class MappingCommands
    {
        public static readonly IAsyncRelayCommand<MappingViewModel> QueryDrives = new AsyncRelayCommand<MappingViewModel>(async parameter =>
        {
            parameter!.Drives.Clear();

            var drives = await Task.Run(() => DriveInfo.GetDrives());

            foreach (DriveInfo drive in drives)
            {
                try
                {
                    parameter.Drives.Add(GetDriveViewModel(parameter, drive));
                }
                catch { }
            }
        });

        const int MAX_PATH = 260;

        private static bool IsVirtualDrive(string driveName)
        {
            Span<char> buffer = stackalloc char[MAX_PATH];

            unsafe
            {
                fixed (char* pBuffer = buffer)
                {
                    return PInvoke.QueryDosDevice(driveName.TrimEnd('\\'), pBuffer, MAX_PATH) != 0 && buffer.StartsWith("\\??\\");
                }
            }
        }

        internal static DriveInfo? GetDriveInfo(char driveLetter) => DriveInfo.GetDrives().FirstOrDefault(d => d.Name.StartsWith(driveLetter));

        internal static DriveViewModel GetDriveViewModel(MappingViewModel mappingViewModel, DriveInfo driveInfo)
        {
            var result = new DriveViewModel
            {
                ParentViewModel = mappingViewModel,
                Letter = driveInfo.Name.First(),
                Label = driveInfo.Name,
                DriveType = driveInfo.DriveType,
                IsVirtual = IsVirtualDrive(driveInfo.Name)
            };

            Task.Run(async () =>
            {
                string? driveName = null;
                string? driveFormat = null;

                try
                {
                    driveName = (await StorageFolder.GetFolderFromPathAsync(driveInfo.Name)).DisplayName;
                }
                catch { }

                try
                {
                    driveFormat = driveInfo.DriveFormat;
                }
                catch (IOException ex) when (ex.HResult == unchecked((int)0x80070015))
                {
                    // ERROR_NOT_READY: Device not ready
                }

                App.MainWindow?.DispatcherQueue.TryEnqueue(DispatcherQueuePriority.Low, () =>
                {
                    result.Label = driveName ?? string.Empty;
                    result.DriveFormat = driveFormat ?? string.Empty;
                });
            });

            return result;
        }

        public static readonly IAsyncRelayCommand<MappingViewModel> AddVirtualDrive = new AsyncRelayCommand<MappingViewModel>(async parameter =>
        {
            var dataContext = new AddDriveViewModel
            {
                ParentViewModel = parameter!
            };
            dataContext.QueryAvailableLetters.Execute(dataContext);

            var dialog = new ContentDialog
            {
                XamlRoot = App.MainWindow?.Content.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                Title = App.ResourceLoader.GetString("AddDriveViewTitle"),
                PrimaryButtonText = App.ResourceLoader.GetString("ContentDialogAdd"),
                CloseButtonText = App.ResourceLoader.GetString("ContentDialogCancel"),
                DefaultButton = ContentDialogButton.Primary,
                PrimaryButtonCommand = dataContext.AddVirtualDrive,
                PrimaryButtonCommandParameter = dataContext,
                Content = new AddDriveView(),
                RequestedTheme = App.RequestedAppTheme,
                DataContext = dataContext
            };

            dialog.Closing += (sender, e) =>
            {
                e.Cancel = e.Result == ContentDialogResult.Primary && dataContext.CancelClose;
            };

            dialog.IsPrimaryButtonEnabled = dialog.PrimaryButtonCommand.CanExecute(dataContext);

            dialog.PrimaryButtonCommand.CanExecuteChanged += (sender, e) =>
            {
                dialog.IsPrimaryButtonEnabled = dialog.PrimaryButtonCommand.CanExecute(dataContext);
            };

            await dialog.ShowAsync();
        });
    }
}