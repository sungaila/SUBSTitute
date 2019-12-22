using PresentationBase;
using Sungaila.SUBSTitute.Core;
using Sungaila.SUBSTitute.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Sungaila.SUBSTitute.Commands
{
    public class MapDriveCommand
        : ViewModelCommand<MainWindowViewModel>
    {
        public override void Execute(MainWindowViewModel parameter)
        {
            if (parameter.SelectedMapping == null || parameter.SelectedMapping.Directory == null)
                return;

            char driveLetter = parameter.SelectedMapping.DriveLetter;

            try
            {
                Win32.DosDevice.MapDrive(driveLetter, parameter.SelectedMapping.Directory);
                parameter.UpdateAvailableMappings(driveLetter);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to map drive letter {driveLetter}.{Environment.NewLine}{ex.Message}", "Failed to map", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public override bool CanExecute(MainWindowViewModel parameter)
        {
            return base.CanExecute(parameter) &&
                parameter.SelectedMapping != null &&
                !String.IsNullOrEmpty(parameter.SelectedMapping.Directory) &&
                (parameter.SelectedMapping.InitialDirectory == null || parameter.SelectedMapping.Directory != parameter.SelectedMapping.InitialDirectory);
        }
    }
}
