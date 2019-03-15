using Microsoft.WindowsAPICodePack.Dialogs;
using Sungaila.SUBSTitute.Core;
using Sungaila.SUBSTitute.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Sungaila.SUBSTitute.Command
{
    public class UnmapAllDrivesCommand
        : ViewModelCommand<MainWindowViewModel>
    {
        public override void Execute(MainWindowViewModel parameter)
        {
            bool failed = false;
            char? selectedDrive = parameter.SelectedMapping?.DriveLetter;

            StringBuilder errorMessage = new StringBuilder("Failed to unmap the following drive letters:");

            foreach (MappingViewModel mapping in parameter.Mappings.Where(mapping => mapping.IsDirectoryMapped))
            {
                char driveLetter = mapping.DriveLetter;

                try
                {
                    Win32.DosDevice.UnmapDrive(driveLetter);
                }
                catch (Exception ex)
                {
                    failed = true;
                    errorMessage.Append($"{Environment.NewLine}{driveLetter} - {ex.Message}");   
                }
            }

            parameter.UpdateAvailableMappings(selectedDrive);

            if (failed)
                MessageBox.Show(errorMessage.ToString(), "Failed to unmap all drives", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public override bool CanExecute(MainWindowViewModel parameter)
        {
            return base.CanExecute(parameter) && parameter.Mappings.Any(mapping => mapping.IsDirectoryMapped);
        }
    }
}
