using Sungaila.SUBSTitute.Core;
using Sungaila.SUBSTitute.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sungaila.SUBSTitute.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var viewModel = new MainWindowViewModel();
            DataContext = viewModel;

            UserSettings userSettings = UserSettings.Load() ?? new UserSettings();

            if (userSettings.LastSelectedDriveLetter != null)
                viewModel.SelectedMapping = viewModel.Mappings.FirstOrDefault(mapping => mapping.DriveLetter == userSettings.LastSelectedDriveLetter)
                    ?? viewModel.Mappings.FirstOrDefault();

            if (!String.IsNullOrEmpty(userSettings.LastBrowserRootDirectory))
                viewModel.BrowserRootDirectory = userSettings.LastBrowserRootDirectory;

            BrowserExpander.IsExpanded = userSettings.IsExpanded;
        }

        private void Window_DpiChanged(object sender, DpiChangedEventArgs e)
        {
            MainWindowViewModel? viewModel = DataContext as MainWindowViewModel;

            if (viewModel == null)
                return;

            viewModel.IsHighDpiContext = e.NewDpi.PixelsPerDip > 1.0;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            MainWindowViewModel? viewModel = DataContext as MainWindowViewModel;

            if (viewModel == null)
                return;

            UserSettings.Save(new UserSettings
            {
                LastSelectedDriveLetter = viewModel.SelectedMapping?.DriveLetter,
                LastBrowserRootDirectory = viewModel.BrowserRootDirectory,
                IsExpanded = BrowserExpander.IsExpanded
            });
        }

        private void BrowserExpander_Expanded(object sender, RoutedEventArgs e)
        {
            SizeToContent = SizeToContent.Manual;

            if (Height < 450)
                Height = 450;
        }

        private void BrowserExpander_Collapsed(object sender, RoutedEventArgs e)
        {
            SizeToContent = SizeToContent.Height;
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListViewItem? listViewItem = sender as ListViewItem;

            if (listViewItem == null)
                return;

            BrowerDirectoryViewModel? contextViewModel = listViewItem.DataContext as BrowerDirectoryViewModel;
            MainWindowViewModel? mainWindowViewModel = contextViewModel?.ParentViewModel as MainWindowViewModel;

            if (contextViewModel == null || mainWindowViewModel == null || mainWindowViewModel.SelectedMapping == null)
                return;

            mainWindowViewModel.BrowserRootDirectory = contextViewModel.FullName;
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            AboutDialog aboutDialog = new AboutDialog();
            aboutDialog.DataContext = DataContext;

            aboutDialog.ShowDialog();
        }
    }
}
