using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.Collections;
using Sungaila.SUBSTitute.Commands;
using System.Collections.ObjectModel;
using WinRT;

namespace Sungaila.SUBSTitute.ViewModels
{
    [GeneratedBindableCustomProperty]
    public partial class MappingViewModel : ViewModel
    {
        private bool _showAllDrives = App.LocalSettings.Values["MappingShowAllDrives"] as bool? ?? true;

        public bool ShowAllDrives
        {
            get => _showAllDrives;
            set
            {
                if (SetProperty(ref _showAllDrives, value))
                {
                    App.LocalSettings.Values["MappingShowAllDrives"] = value;
                    DrivesFiltered.RefreshFilter();
                }
            }
        }

        private int _selectedIndex = App.LocalSettings.Values["MappingNavSelectedIndex"] as int? ?? 1;

        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                if (SetProperty(ref _selectedIndex, value))
                {
                    App.LocalSettings.Values["MappingNavSelectedIndex"] = value;
                }
            }
        }

        public ObservableCollection<DriveViewModel> Drives { get; } = [];

        public AdvancedCollectionView DrivesFiltered { get; } = [];

        public IRelayCommand QueryDrives { get; } = MappingCommands.QueryDrives;

        public IRelayCommand AddVirtualDrive { get; } = MappingCommands.AddVirtualDrive;

        public MappingViewModel()
        {
            DrivesFiltered.Filter = FilterDrives;
            DrivesFiltered.Source = Drives;
            DrivesFiltered.SortDescriptions.Add(new SortDescription(nameof(DriveViewModel.Letter), SortDirection.Ascending));
        }

        private bool FilterDrives(object obj)
        {
            if (obj is not DriveViewModel drive)
                return false;

            return ShowAllDrives || drive.IsVirtual;
        }
    }
}