using PresentationBase;
using Sungaila.SUBSTitute.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace Sungaila.SUBSTitute.ViewModels
{
    [DebuggerDisplay("MappingViewModel {DriveLetter}, {Directory}")]
    public class MappingViewModel
        : ViewModel
    {
        public MappingViewModel()
        {
            PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(ParentViewModel))
                    UpdateIcon();
            };
        }

        private char _driveLetter;

        public char DriveLetter
        {
            get => _driveLetter;
            set
            {
                if (!SetProperty(ref _driveLetter, value))
                    return;

                UpdateIcon();
            }
        }

        private string? _directory;

        public string? Directory
        {
            get => _directory;
            set => SetProperty(ref _directory, value != null && !value.EndsWith(":\\") ? value.TrimEnd('\\') : value);
        }

        private string? _initialDirectory;

        public string? InitialDirectory
        {
            get => _initialDirectory;
            set
            {
                SetProperty(ref _initialDirectory, value != null && !value.EndsWith(":\\") ? value.TrimEnd('\\') : value);
                Directory = InitialDirectory;
                RaisePropertyChanged(nameof(IsDirectoryMapped));
            }
        }

        public bool IsDirectoryMapped
        {
            get => InitialDirectory != null;
        }

        private Icon? _icon;

        public Icon? Icon
        {
            get => _icon;
            set => SetProperty(ref _icon, value);
        }

        public void UpdateIcon()
        {
            bool? isHighDpiContext = (ParentViewModel as MainWindowViewModel)?.IsHighDpiContext;

            Icon = isHighDpiContext.HasValue && isHighDpiContext.Value
                ? Win32.IconHelper.GetLargeIcon($"{DriveLetter}:\\")
                : Win32.IconHelper.GetSmallIcon($"{DriveLetter}:\\");
        }
    }
}
