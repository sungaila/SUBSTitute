using Microsoft.UI.Xaml.Controls;
using Sungaila.SUBSTitute.ViewModels;

namespace Sungaila.SUBSTitute.Views
{
    public sealed partial class TileView : Page
    {
        private MappingViewModel? Data => DataContext as MappingViewModel;

        public TileView()
        {
            this.InitializeComponent();
        }
    }
}