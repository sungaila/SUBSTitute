using Sungaila.SUBSTitute.Core;
using Sungaila.SUBSTitute.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sungaila.SUBSTitute.Command
{
    public class BrowserGotoParentDirectoryCommand
        : ViewModelCommand<MainWindowViewModel>
    {
        public override void Execute(MainWindowViewModel parameter)
        {
            if (String.IsNullOrEmpty(parameter.BrowserRootDirectory))
                return;

            DirectoryInfo dirInfo = new DirectoryInfo(parameter.BrowserRootDirectory);

            if (!dirInfo.Exists || dirInfo.Parent == null)
                return;

            parameter.BrowserRootDirectory = dirInfo.Parent.FullName;
        }

        public override bool CanExecute(MainWindowViewModel parameter)
        {
            if (!base.CanExecute(parameter) || String.IsNullOrEmpty(parameter.BrowserRootDirectory))
                return false;

            DirectoryInfo dirInfo = new DirectoryInfo(parameter.BrowserRootDirectory);
            
            return dirInfo.Exists && dirInfo.Parent != null;
        }
    }
}
