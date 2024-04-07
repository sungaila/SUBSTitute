using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.System.Com;
using Windows.Win32.UI.Shell;
using WinUIEx;

namespace Sungaila.SUBSTitute.Views
{
    public sealed partial class AddDriveView : Page
    {
        public AddDriveView()
        {
            this.InitializeComponent();
            AdminIcon.Visibility = !App.IsElevated ? Visibility.Visible : Visibility.Collapsed;
        }

        private unsafe void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PInvoke.CoCreateInstance(
                    typeof(FileOpenDialog).GUID,
                    null,
                    CLSCTX.CLSCTX_INPROC_SERVER,
                    out IFileOpenDialog openDialog).ThrowOnFailure();
                openDialog.SetOptions(FILEOPENDIALOGOPTIONS.FOS_HIDEMRUPLACES | FILEOPENDIALOGOPTIONS.FOS_PICKFOLDERS | FILEOPENDIALOGOPTIONS.FOS_NODEREFERENCELINKS);
                openDialog.Show(new HWND(App.MainWindow!.GetWindowHandle()));
                openDialog.GetResult(out IShellItem item);
                item.GetDisplayName(SIGDN.SIGDN_FILESYSPATH, out PWSTR selectedFolder);

                PathTextBox.Text = selectedFolder.ToString();
                Marshal.FreeCoTaskMem(new IntPtr(selectedFolder.Value));
            }
            catch (COMException ex) when ((ex.ErrorCode & 0x000004c7) == 0x000004c7)
            {
                // ERROR_CANCELLED: The operation was canceled by the user.
                return;
            }
        }
    }
}