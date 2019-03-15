using Sungaila.SUBSTitute.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Sungaila.SUBSTitute
{
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            if (MessageBox.Show(MainWindow,
                $"The following unexpected error has occured:{Environment.NewLine}{Environment.NewLine}{e.Exception.Message}{Environment.NewLine}{Environment.NewLine}[OK] will terminate this application.{Environment.NewLine}[Cancel] will keep the application running but it might be unstable from now on.",
                "Unexpected error",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Error) != MessageBoxResult.OK)
                e.Handled = true;
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            MainWindowViewModel? viewModel = MainWindow.DataContext as MainWindowViewModel;

            if (viewModel == null)
                return;

            UserSettings.Save(new UserSettings
            {
                LastSelectedDriveLetter = viewModel.SelectedMapping?.DriveLetter,
                LastBrowserRootDirectory = viewModel.BrowserRootDirectory
            });
        }
    }
}
