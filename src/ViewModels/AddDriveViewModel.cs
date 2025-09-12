using CommunityToolkit.Mvvm.Input;
using Sungaila.SUBSTitute.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using WinRT;

namespace Sungaila.SUBSTitute.ViewModels
{
    [GeneratedBindableCustomProperty]
    public partial class AddDriveViewModel : ViewModel
    {
        public required MappingViewModel ParentViewModel { get; init; }

        public ObservableCollection<LetterViewModel> AvailableLetters { get; } = [];

        [Required]
        public LetterViewModel? SelectedLetter
        {
            get => field;
            set
            {
                if (SetProperty(ref field, value))
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

        public bool IsPermanent
        {
            get => field;
            set => SetProperty(ref field, value);
        }

        public bool CancelClose { get; set; }

        public IRelayCommand QueryAvailableLetters { get; } = AddDriveCommands.QueryAvailableLetters;

        public IRelayCommand AddVirtualDrive { get; } = AddDriveCommands.AddVirtualDrive;
    }
}