using Sungaila.SUBSTitute.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32;

namespace Sungaila.SUBSTitute.Win32
{
    /// <summary>
    /// Wraps API calls for mapping directories to virtual drives.
    /// </summary>
    public static class DosDevice
    {
        /// <summary>
        /// Defines, redefines, or deletes MS-DOS device names.
        /// <see href="https://docs.microsoft.com/en-us/windows/desktop/api/fileapi/nf-fileapi-definedosdevicew"/>
        /// </summary>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool DefineDosDevice(int dwFlags, string lpDeviceName, string? lpTargetPath);

        /// <summary>
        /// Retrieves information about MS-DOS device names. The function can obtain the current mapping for a particular MS-DOS device name. The function can also obtain a list of all existing MS-DOS device names.
        /// <see href="https://docs.microsoft.com/en-us/windows/desktop/api/fileapi/nf-fileapi-querydosdevicew"/>
        /// </summary>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int QueryDosDevice(string lpDeviceName, StringBuilder lpTargetPath, int ucchMax);

        [Flags]
        private enum DefineDosDeviceFlags
        {
            None = 0,

            /// <summary>
            /// Uses the lpTargetPath string as is. Otherwise, it is converted from an MS-DOS path to a path.
            /// </summary>
            DDD_RAW_TARGET_PATH = 1 << 0,

            /// <summary>
            /// Removes the specified definition for the specified device. 
            /// </summary>
            DDD_REMOVE_DEFINITION = 1 << 1,

            /// <summary>
            /// If this value is specified along with <see cref="DDD_REMOVE_DEFINITION"/>, the function will use an exact match to determine which mapping to remove.
            /// </summary>
            DDD_EXACT_MATCH_ON_REMOVE = 1 << 2,

            /// <summary>
            /// Do not broadcast the <see href="https://docs.microsoft.com/de-de/windows/desktop/winmsg/wm-settingchange">WM_SETTINGCHANGE</see> message. By default, this message is broadcast to notify the shell and applications of the change.
            /// </summary>
            DDD_NO_BROADCAST_SYSTEM = 1 << 3
        }

        private static void SaveMappingToRegistry(char driveLetter, string path)
        {
            var subkey = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
            subkey.SetValue($"{driveLetter} Drive", $"subst {driveLetter}: {path}");
        }

        private static void RemoveMappingFromRegistry(char driveLetter)
        {
            var subkey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
            if (subkey != null)
            {
                subkey.DeleteValue($"{driveLetter} Drive", false); 
            }
        }

        /// <summary>
        /// Maps the given directory path to a drive letter.
        /// </summary>
        /// <param name="driveLetter">A drive letter (from A to Z).</param>
        /// <param name="path">The directory path to map.</param>
        public static void MapDrive(char driveLetter, string path)
        {
            driveLetter = char.ToUpperInvariant(driveLetter);

            if (driveLetter < 'A' || driveLetter > 'Z')
                throw new ArgumentOutOfRangeException(nameof(driveLetter), "The drive letter must be a letter from A to Z.");

            if (path == null)
                throw new ArgumentNullException(nameof(path));

            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException();

            if (!DefineDosDevice((int)DefineDosDeviceFlags.DDD_RAW_TARGET_PATH, $"{driveLetter}:", "\\??\\" + new DirectoryInfo(path).FullName))
                throw new Win32Exception(Marshal.GetLastWin32Error());

            SaveMappingToRegistry(driveLetter, path);
        }

        /// <summary>
        /// Unmaps a given drive letter.
        /// </summary>
        /// <param name="driveLetter">A drive letter (from A to Z).</param>
        public static void UnmapDrive(char driveLetter)
        {
            driveLetter = char.ToUpperInvariant(driveLetter);

            if (driveLetter < 'A' || driveLetter > 'Z')
                throw new ArgumentOutOfRangeException(nameof(driveLetter), "The drive letter must be a letter from A to Z.");

            if (!DefineDosDevice((int)DefineDosDeviceFlags.DDD_REMOVE_DEFINITION, $"{driveLetter}:", null))
                throw new Win32Exception(Marshal.GetLastWin32Error());

            RemoveMappingFromRegistry(driveLetter);
        }

        private const int MAX_PATH = 248;

        private static readonly int DRIVE_UNC_LENGTH = "\\??\\".Length;

        /// <summary>
        /// Gets the mapped directory path for a given drive letter (if there is one).
        /// </summary>
        /// <param name="driveLetter">A drive letter (from A to Z).</param>
        /// <returns>Returns the mapped directory path. Returns <c>null</c> if the drive letter was not mapped.</returns>
        public static string? GetDriveMapping(char driveLetter)
        {
            driveLetter = char.ToUpperInvariant(driveLetter);

            if (driveLetter < 'A' || driveLetter > 'Z')
                throw new ArgumentOutOfRangeException(nameof(driveLetter), "The drive letter must be a letter from A to Z.");

            var buffer = new StringBuilder(MAX_PATH + DRIVE_UNC_LENGTH);

            if (QueryDosDevice($"{driveLetter}:", buffer, buffer.Capacity) == 0)
            {
                int error = Marshal.GetLastWin32Error();

                if (error == 0 || error == 2) // ERROR_FILE_NOT_FOUND: The system cannot find the file specified.
                    return null;

                throw new Win32Exception(error);
            }

            // there is a mapping but it is not a SUBST directory
            if (!buffer.ToString().StartsWith("\\??\\"))
                return null;

            return buffer.ToString().Substring(DRIVE_UNC_LENGTH);
        }

        public static IEnumerable<MappingViewModel> GetAvailableDrives()
        {
            for (char drive = 'A'; drive <= 'Z'; drive++)
            {
                var buffer = new StringBuilder(MAX_PATH + DRIVE_UNC_LENGTH);

                if (QueryDosDevice($"{drive}:", buffer, buffer.Capacity) == 0)
                {
                    int error = Marshal.GetLastWin32Error();

                    if (error == 2) // ERROR_FILE_NOT_FOUND: The system cannot find the file specified.
                        yield return new MappingViewModel { DriveLetter = drive };
                }

                if (buffer.ToString().StartsWith("\\??\\"))
                    yield return new MappingViewModel
                    {
                        DriveLetter = drive,
                        InitialDirectory = buffer.ToString().Substring(DRIVE_UNC_LENGTH)
                    };
            }
        }
    }
}
