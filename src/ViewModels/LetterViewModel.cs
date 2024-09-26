namespace Sungaila.SUBSTitute.ViewModels
{
    public partial class LetterViewModel : ViewModel
    {
        private char _name;

        public char Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
    }
}