using WinRT;

namespace Sungaila.SUBSTitute.ViewModels
{
    [GeneratedBindableCustomProperty]
    public partial class LetterViewModel : ViewModel
    {
        public char Name
        {
            get => field;
            set => SetProperty(ref field, value);
        }
    }
}