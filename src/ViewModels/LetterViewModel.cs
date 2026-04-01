using CommunityToolkit.Mvvm.ComponentModel;
using WinRT;

namespace Sungaila.SUBSTitute.ViewModels
{
    [GeneratedBindableCustomProperty]
    public partial class LetterViewModel : ViewModel
    {
        [ObservableProperty]
        public partial char Name { get; set; }
    }
}