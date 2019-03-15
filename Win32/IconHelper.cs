using Sungaila.SUBSTitute.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Sungaila.SUBSTitute.Win32
{
    public static class IconHelper
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public IntPtr iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }

        [Flags]
        private enum GetFileInfoFlags : uint
        {
            SHGFI_LARGEICON = 0x0,
            SHGFI_SMALLICON = 0x1,
            SHGFI_ICON = 0x100,
        }

        /// <summary>
        /// Retrieves information about an object in the file system, such as a file, folder, directory, or drive root.
        /// <see href="https://docs.microsoft.com/en-us/windows/desktop/api/shellapi/nf-shellapi-shgetfileinfoa"/>
        /// </summary>
        [DllImport("shell32.dll", CharSet=CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);

        public static Icon? GetSmallIcon(string path)
        {
            return GetIcon(path, GetFileInfoFlags.SHGFI_ICON | GetFileInfoFlags.SHGFI_SMALLICON);
        }

        public static Icon? GetLargeIcon(string path)
        {
            return GetIcon(path, GetFileInfoFlags.SHGFI_ICON | GetFileInfoFlags.SHGFI_LARGEICON);
        }

        private static Icon? GetIcon(string path, GetFileInfoFlags flags)
        {
            SHFILEINFO shinfo = new SHFILEINFO();

            try
            {
                SHGetFileInfo(path, 0, ref shinfo,
                    (uint)Marshal.SizeOf(shinfo),
                    (uint)flags);

                return shinfo.hIcon.ToInt64() != 0x0 ? Icon.FromHandle(shinfo.hIcon) : null;
            }
            catch
            {
                return null;
            }
        }
    }
}
