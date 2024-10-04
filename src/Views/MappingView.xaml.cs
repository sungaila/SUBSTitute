using CommunityToolkit.WinUI.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Sungaila.SUBSTitute.ViewModels;
using System;

namespace Sungaila.SUBSTitute.Views
{
    public sealed partial class MappingView : Page
    {
        private MainWindow MainWindow => App.MainWindow!;

        private MainViewModel? Data => DataContext as MainViewModel;

        public MappingView()
        {
            this.InitializeComponent();
        }

        private void Segmented_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is not Segmented segmented || segmented.SelectedItem is not FrameworkElement element || element.Tag is not Type type)
                return;

            ContentFrame?.Navigate(type);
        }
    }
}