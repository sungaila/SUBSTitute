using Microsoft.WindowsAPICodePack.Dialogs;
using PresentationBase;
using Sungaila.SUBSTitute.Core;
using Sungaila.SUBSTitute.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sungaila.SUBSTitute.Commands
{
    public class SelectBrowserDirectoryCommand
        : ViewModelCommand<MainWindowViewModel>
    {
        public override void Execute(MainWindowViewModel parameter)
        {
            var openDirectoryDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                InitialDirectory = parameter.BrowserRootDirectory
            };

            if (openDirectoryDialog.ShowDialog() == CommonFileDialogResult.Ok)
                parameter.BrowserRootDirectory = openDirectoryDialog.FileName;
        }
    }
}
