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

        public bool RemovePermanent
        {
            get => field;
            set => SetProperty(ref field, value);
        }

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