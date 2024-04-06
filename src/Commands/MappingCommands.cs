using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Sungaila.SUBSTitute.ViewModels;
using Sungaila.SUBSTitute.Views;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Win32;

namespace Sungaila.SUBSTitute.Commands
{
    public static class MappingCommands
    {
        public static IAsyncRelayCommand<MappingViewModel> QueryDrives { get; } = new AsyncRelayCommand<MappingViewModel>(async parameter =>
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
                fixed (char* pText = buffer)
                {
                    return PInvoke.QueryDosDevice(driveName.TrimEnd('\\'), pText, MAX_PATH) != 0 && buffer.StartsWith("\\??\\");
                }
            }
        }

        internal static DriveInfo? GetDriveInfo(char driveLetter) => DriveInfo.GetDrives().FirstOrDefault(d => d.Name.StartsWith(driveLetter));

        internal static DriveViewModel GetDriveViewModel(MappingViewModel mappingViewModel, DriveInfo driveInfo)
        {
            string volumeLabel = string.Empty;
            string driveFormat = string.Empty;

            if (driveInfo.IsReady)
            {
                volumeLabel = driveInfo.VolumeLabel;
                driveFormat = driveInfo.DriveFormat;
            }

            return new DriveViewModel
            {
                ParentViewModel = mappingViewModel,
                Letter = driveInfo.Name.First(),
                Label = volumeLabel,
                DriveFormat = driveFormat,
                DriveType = driveInfo.DriveType,
                IsVirtual = IsVirtualDrive(driveInfo.Name)
            };
        }

        public static IAsyncRelayCommand<MappingViewModel> Add { get; } = new AsyncRelayCommand<MappingViewModel>(async parameter =>
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
                Content = new AddDriveView(),
                UseLayoutRounding = true,
                RequestedTheme = App.RequestedAppTheme,
                DataContext = dataContext
            };

            if (await dialog.ShowAsync() != ContentDialogResult.Primary)
                return;

            dataContext.ConnectVirtualDrive.Execute(dataContext);
        });
    }
}