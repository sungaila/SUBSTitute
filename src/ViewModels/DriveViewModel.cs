using Sungaila.SUBSTitute.Commands;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace Sungaila.SUBSTitute.ViewModels
{
    public partial class DriveViewModel : ViewModel
    {
        [GeneratedRegex(@" \([A-Z]:\)")]
        private static partial Regex AppendedDriveLetterRegex();

        [GeneratedRegex(@"[A-Z]:\\")]
        private static partial Regex DriveLetterRegex();

        public required MappingViewModel ParentViewModel { get; init; }

        public char Letter
        {
            get => field;
            set
            {
                if (SetProperty(ref field, value))
                {
                    OnPropertyChanged(nameof(DriveName));
                }
            }
        }

        public string DriveName
        {
            get
            {
                if (!string.IsNullOrEmpty(DisplayName))
                {
                    if (AppendedDriveLetterRegex().IsMatch(DisplayName))
                    {
                        return DisplayName[..^5];
                    }
                    else if (!DriveLetterRegex().IsMatch(DisplayName))
                    {
                        return DisplayName;
                    }
                }

                return string.Empty;
            }
        }

        private string _label = string.Empty;

        public string Label
        {
            get => _label;
            set
            {
                if (SetProperty(ref _label, value))
                {
                    OnPropertyChanged(nameof(DriveName));
                }
            }
        }

        private string _displayName = string.Empty;

        public string DisplayName
        {
            get => !string.IsNullOrEmpty(_displayName) ? _displayName : _label;
            set
            {
                if (SetProperty(ref _displayName, value))
                {
                    OnPropertyChanged(nameof(DriveName));
                }
            }
        }

        private string _driveFormat = string.Empty;

        public string DriveFormat
        {
            get => _driveFormat;
            set => SetProperty(ref _driveFormat, value);
        }

        public DriveType DriveType
        {
            get => field;
            set
            {
                if (SetProperty(ref field, value))
                {
                    OnPropertyChanged(nameof(DriveTypeLocalized));
                }
            }
        }

        public string DriveTypeLocalized
        {
            get
            {
                if (IsVirtual)
                    return App.ResourceLoader.GetString(nameof(IsVirtual));

                return App.ResourceLoader.GetString($"{nameof(DriveType)}+{Enum.GetName(DriveType)}");
            }
        }

        private bool _isVirtual;

        public bool IsVirtual
        {
            get => _isVirtual;
            set
            {
                if (SetProperty(ref _isVirtual, value))
                {
                    OnPropertyChanged(nameof(DriveTypeLocalized));
                }
            }
        }

        public ICommand Open { get; } = DriveCommands.Open;

        public ICommand RemoveVirtualDrive { get; } = DriveCommands.RemoveVirtualDrive;
    }
}