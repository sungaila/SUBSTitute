using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Win32;
using WinRT;

namespace Sungaila.SUBSTitute.ViewModels
{
    [GeneratedBindableCustomProperty]
    public partial class RemoveDriveViewModel : ViewModel
    {
        public required MappingViewModel ParentViewModel { get; init; }

        public DriveViewModel DriveToRemove { get; }

        public bool IsPermanent { get; }

        [ObservableProperty]
        public partial bool RemovePermanent { get; set; }

        public bool CancelClose { get; set; }

        public RemoveDriveViewModel(DriveViewModel driveToRemove)
        {
            DriveToRemove = driveToRemove;

            using var currentUserKey = Registry.LocalMachine;
            using var subKey = currentUserKey.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Session Manager\DOS Devices");

            IsPermanent = subKey?.GetValue($"{DriveToRemove.Letter}:") is string;
        }
    }
}