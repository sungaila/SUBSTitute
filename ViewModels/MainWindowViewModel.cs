using Sungaila.SUBSTitute.Command;
using Sungaila.SUBSTitute.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace Sungaila.SUBSTitute.ViewModels
{
    public class MainWindowViewModel
        : ViewModel
    {
        public MainWindowViewModel()
        {
            Mappings = new ObservableViewModelCollection<MappingViewModel>(this);
            Mappings.Observe(() => RaisePropertyChanged(nameof(Mappings)), nameof(MappingViewModel.DriveLetter), nameof(MappingViewModel.Directory));
            BrowserDirectories = new ObservableViewModelCollection<BrowerDirectoryViewModel>(this);

            AddCommands(new SelectMappedDirectoryCommand(),
                new SelectBrowserDirectoryCommand(),
                new BrowserGotoParentDirectoryCommand(),
                new MapDriveCommand(),
                new UnmapDriveCommand(),
                new UnmapAllDrivesCommand());

            UpdateAvailableMappings();
        }

        public void UpdateAvailableMappings(char? driveLetterToSelect = null)
        {
            Mappings.Clear();
            
            foreach (MappingViewModel mapping in Win32.DosDevice.GetAvailableDrives())
                Mappings.Add(mapping);
            
            SelectedMapping = driveLetterToSelect != null
                ? Mappings.FirstOrDefault(mapping => mapping.DriveLetter == driveLetterToSelect)
                : Mappings.FirstOrDefault(mapping => mapping.IsDirectoryMapped) ?? Mappings.FirstOrDefault();
        }

        public ObservableViewModelCollection<MappingViewModel> Mappings { get; }

        private MappingViewModel? _selectedMapping;

        public MappingViewModel? SelectedMapping
        {
            get => _selectedMapping;
            set
            {
                var oldValue = SelectedMapping;
                
                if (!SetViewModelProperty(ref _selectedMapping, value))
                    return;

                if (oldValue != null)
                    oldValue.Directory = oldValue.InitialDirectory;

                if (_selectedMapping == null)
                {
                    return;
                }
                else if (_selectedMapping.Directory == null)
                {
                    SelectedBrowserDirectory = null;
                    return;
                }

                DirectoryInfo dirInfo = new DirectoryInfo(_selectedMapping.Directory);

                if (!dirInfo.Exists || dirInfo.Parent == null || !dirInfo.Parent.Exists)
                    return;

                BrowserRootDirectory = dirInfo.Parent.FullName;
                SelectedBrowserDirectory = BrowserDirectories.FirstOrDefault(dir => dir.FullName != null && dir.FullName.TrimEnd('\\') == dirInfo.FullName.TrimEnd('\\'));
            }
        }

        private string? _browserRootDirectory;

        public string? BrowserRootDirectory
        {
            get => _browserRootDirectory;
            set
            {
                if (!SetProperty(ref _browserRootDirectory, value != null && !value.EndsWith(":\\") ? value.TrimEnd('\\') : value))
                    return;

                UpdateBrowserDirectories();
            }
        }

        private void UpdateBrowserDirectories()
        {
            if (!Directory.Exists(BrowserRootDirectory))
                return;

            BrowserDirectories.Clear();

            foreach (string subDirectory in Directory.GetDirectories(BrowserRootDirectory, "*.*", SearchOption.TopDirectoryOnly))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(subDirectory);

                if (dirInfo.Attributes.HasFlag(FileAttributes.System))
                    continue;

                BrowserDirectories.Add(new BrowerDirectoryViewModel
                {
                    Name = dirInfo.Name,
                    FullName = dirInfo.FullName
                });
            }
        }

        public ObservableViewModelCollection<BrowerDirectoryViewModel> BrowserDirectories { get; }

        private BrowerDirectoryViewModel? _selectedBrowserDirectory;

        public BrowerDirectoryViewModel? SelectedBrowserDirectory
        {
            get => _selectedBrowserDirectory;
            set
            {
                if (!SetViewModelProperty(ref _selectedBrowserDirectory, value) || 
                    _selectedBrowserDirectory == null ||
                    SelectedMapping == null)
                    return;

                SelectedMapping.Directory = _selectedBrowserDirectory.FullName;
            }
        }

        private bool _isHighDpiContext;

        public bool IsHighDpiContext
        {
            get => _isHighDpiContext;
            set
            {
                if (!SetProperty(ref _isHighDpiContext, value))
                    return;

                foreach (MappingViewModel mapping in Mappings)
                    mapping.UpdateIcon();

                foreach (BrowerDirectoryViewModel directory in BrowserDirectories)
                    directory.UpdateIcon();
            }
        }
    }
}
