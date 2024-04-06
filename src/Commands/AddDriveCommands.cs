using CommunityToolkit.Mvvm.Input;
using Sungaila.SUBSTitute.ViewModels;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.Storage.FileSystem;

namespace Sungaila.SUBSTitute.Commands
{
    public static class AddDriveCommands
    {
        public static readonly IRelayCommand<AddDriveViewModel> QueryAvailableLetters = new RelayCommand<AddDriveViewModel>(parameter =>
        {
            parameter!.AvailableLetters.Clear();

            var letters = new List<LetterViewModel>();

            for (char i = 'A'; i <= 'Z'; i++)
            {
                letters.Add(new LetterViewModel { Name = i });
            }

            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                try
                {
                    var existingLetter = letters.FirstOrDefault(l => l.Name == drive.Name.First());

                    if (existingLetter != null)
                        letters.Remove(existingLetter);
                }
                catch { }
            }

            foreach (var letter in letters)
            {
                parameter.AvailableLetters.Add(letter);
            }

            parameter.SelectedLetter = parameter.AvailableLetters.FirstOrDefault();
        });

        public static readonly IRelayCommand<AddDriveViewModel> AddVirtualDrive = new RelayCommand<AddDriveViewModel>(parameter =>
        {
            parameter!.CancelClose = false;

            if (parameter?.SelectedLetter?.Name is not char letter || string.IsNullOrEmpty(parameter.SelectedPath))
                return;

            var selectedPath = Path.GetFullPath(parameter.SelectedPath.Trim('\"'));

            if (!Path.Exists(selectedPath) || File.Exists(selectedPath))
            {
                throw new DirectoryNotFoundException();
            }

            if (parameter.IsPermanent)
            {
                try
                {
                    Process.Start(new ProcessStartInfo(
                        $"reg.exe",
                        $"add \"HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\DOS Devices\" /v \"{letter}:\" /t REG_SZ /d \"\\\\??\\{selectedPath}\" /f")
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

            AddDosDevice($"{letter}:", selectedPath);

            if (MappingCommands.GetDriveInfo(letter) is not DriveInfo newDriveInfo)
                return;

            parameter.ParentViewModel.Drives.Add(MappingCommands.GetDriveViewModel(parameter.ParentViewModel, newDriveInfo));
        },
        parameter => parameter?.SelectedLetter != null && !string.IsNullOrEmpty(parameter?.SelectedPath));

        private static void AddDosDevice(string driveName, string targetPath)
        {
            if (!PInvoke.DefineDosDevice(DEFINE_DOS_DEVICE_FLAGS.DDD_RAW_TARGET_PATH, driveName, $"\\??\\{targetPath}"))
                throw new Win32Exception(Marshal.GetLastWin32Error());
        }
    }
}