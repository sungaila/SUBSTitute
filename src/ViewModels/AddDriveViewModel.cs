using CommunityToolkit.Mvvm.Input;
using Sungaila.SUBSTitute.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Sungaila.SUBSTitute.ViewModels
{
    public partial class AddDriveViewModel : ViewModel
    {
        public required MappingViewModel ParentViewModel { get; init; }

        public ObservableCollection<LetterViewModel> AvailableLetters { get; } = [];

        private LetterViewModel? _selectedLetter;

        [Required]
        public LetterViewModel? SelectedLetter
        {
            get => _selectedLetter;
            set
            {
                if (SetProperty(ref _selectedLetter, value))
                {
                    AddVirtualDrive.NotifyCanExecuteChanged();
                }
            }
        }

        private string _selectedPath = string.Empty;

        [Required]
        public string SelectedPath
        {
            get => _selectedPath;
            set
            {
                if (SetProperty(ref _selectedPath, value))
                {
                    AddVirtualDrive.NotifyCanExecuteChanged();
                }
            }
        }

        private bool _isPermanent;

        public bool IsPermanent
        {
            get => _isPermanent;
            set => SetProperty(ref _isPermanent, value);
        }

        public bool CancelClose { get; set; }

        public IRelayCommand QueryAvailableLetters { get; } = AddDriveCommands.QueryAvailableLetters;

        public IRelayCommand AddVirtualDrive { get; } = AddDriveCommands.AddVirtualDrive;
    }
}