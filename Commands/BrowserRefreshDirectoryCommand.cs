using Sungaila.SUBSTitute.Core;
using Sungaila.SUBSTitute.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sungaila.SUBSTitute.Command
{
    public class BrowserRefreshDirectoryCommand
        : ViewModelCommand<MainWindowViewModel>
    {
        public override void Execute(MainWindowViewModel parameter)
        {
            if (String.IsNullOrEmpty(parameter.BrowserRootDirectory))
                return;

            parameter.UpdateBrowserDirectories();
        }

        public override bool CanExecute(MainWindowViewModel parameter)
        {
            return base.CanExecute(parameter) && !String.IsNullOrEmpty(parameter.BrowserRootDirectory);
        }
    }
}
