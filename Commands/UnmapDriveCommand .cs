using PresentationBase;
using Sungaila.SUBSTitute.Core;
using Sungaila.SUBSTitute.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Sungaila.SUBSTitute.Commands
{
    public class UnmapDriveCommand
        : ViewModelCommand<MainWindowViewModel>
    {
        public override void Execute(MainWindowViewModel parameter)
        {
            if (parameter.SelectedMapping == null)
                return;

            char driveLetter = parameter.SelectedMapping.DriveLetter;

            try
            {
                Win32.DosDevice.UnmapDrive(driveLetter);
                parameter.UpdateAvailableMappings(driveLetter);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to unmap drive letter {driveLetter}.{Environment.NewLine}{ex.Message}", "Failed to unmap", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public override bool CanExecute(MainWindowViewModel parameter)
        {
            return base.CanExecute(parameter) && parameter.SelectedMapping != null && parameter.SelectedMapping.IsDirectoryMapped;
        }
    }
}
