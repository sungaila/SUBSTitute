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

        private char _letter;

        public char Letter
        {
            get => _letter;
            set
            {
                if (SetProperty(ref _letter, value))
                {
                    OnPropertyChanged(nameof(Path));
                    OnPropertyChanged(nameof(DriveName));
                }
            }
        }

        public string Path => $"{Letter}:\\";

        public string DriveName
        {
            get
            {
                if (!string.IsNullOrEmpty(Label))
                {
                    if (AppendedDriveLetterRegex().IsMatch(Label))
                    {
                        return Label[..^5];
                    }
                    else if (!DriveLetterRegex().IsMatch(Label))
                    {
                        return Label;
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

        private string _driveFormat = string.Empty;

        public string DriveFormat
        {
            get => _driveFormat;
            set => SetProperty(ref _driveFormat, value);
        }

        private DriveType _driveType;

        public DriveType DriveType
        {
            get => _driveType;
            set
            {
                if (SetProperty(ref _driveType, value))
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