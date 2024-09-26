using Microsoft.Win32;

namespace Sungaila.SUBSTitute.ViewModels
{
    public partial class RemoveDriveViewModel : ViewModel
    {
        public required MappingViewModel ParentViewModel { get; init; }

        public DriveViewModel DriveToRemove { get; }

        public bool IsPermanent { get; }

        private bool _removePermanent;

        public bool RemovePermanent
        {
            get => _removePermanent;
            set => SetProperty(ref _removePermanent, value);
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