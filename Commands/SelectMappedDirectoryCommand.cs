using Microsoft.WindowsAPICodePack.Dialogs;
using Sungaila.SUBSTitute.Core;
using Sungaila.SUBSTitute.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sungaila.SUBSTitute.Command
{
    public class SelectMappedDirectoryCommand
        : ViewModelCommand<MainWindowViewModel>
    {
        public override void Execute(MainWindowViewModel parameter)
        {
            if (parameter.SelectedMapping == null)
                return;

            DirectoryInfo? dirInfo = parameter.SelectedMapping.Directory != null
                ? new DirectoryInfo(parameter.SelectedMapping.Directory)
                : null;

            var openDirectoryDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                InitialDirectory = dirInfo != null && dirInfo.Exists && dirInfo.Parent != null ? dirInfo.Parent.FullName : null
            };

            if (openDirectoryDialog.ShowDialog() == CommonFileDialogResult.Ok)
                parameter.SelectedMapping.Directory = openDirectoryDialog.FileName;
        }

        public override bool CanExecute(MainWindowViewModel parameter)
        {
            return base.CanExecute(parameter) && parameter.SelectedMapping != null;
        }
    }
}
