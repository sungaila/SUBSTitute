using Microsoft.UI.Xaml.Controls;
using Sungaila.SUBSTitute.ViewModels;

namespace Sungaila.SUBSTitute.Views
{
    public sealed partial class ListView : Page
    {
        private MappingViewModel? Data => DataContext as MappingViewModel;

        public ListView()
        {
            this.InitializeComponent();
        }
    }
}