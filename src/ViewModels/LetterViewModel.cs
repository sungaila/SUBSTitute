namespace Sungaila.SUBSTitute.ViewModels
{
    public class LetterViewModel : ViewModel
    {
        private char _name;

        public char Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
    }
}