using Sungaila.SUBSTitute.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Sungaila.SUBSTitute.ViewModels
{
    public class AddDriveViewModel : ViewModel
    {
        public required MappingViewModel ParentViewModel { get; init; }

        public ObservableCollection<LetterViewModel> AvailableLetters { get; } = [];

        private LetterViewModel? _selectedLetter;

        public LetterViewModel? SelectedLetter
        {
            get => _selectedLetter;
            set => SetProperty(ref _selectedLetter, value);
        }

        private string _selectedPath = string.Empty;

        public string SelectedPath
        {
            get => _selectedPath;
            set => SetProperty(ref _selectedPath, value);
        }

        private bool _isPermanent;

        public bool IsPermanent
        {
            get => _isPermanent;
            set => SetProperty(ref _isPermanent, value);
        }

        public ICommand QueryAvailableLetters { get; } = AddDriveCommands.QueryAvailableLetters;

        public ICommand ConnectVirtualDrive { get; } = AddDriveCommands.ConnectVirtualDrive;
    }
}