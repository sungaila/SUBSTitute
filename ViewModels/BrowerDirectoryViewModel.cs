using PresentationBase;
using Sungaila.SUBSTitute.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Sungaila.SUBSTitute.ViewModels
{
    [DebuggerDisplay("BrowerDirectoryViewModel {Name}")]
    public class BrowerDirectoryViewModel
        : ViewModel, ISelectable
    {
        public BrowerDirectoryViewModel()
        {
            PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName != nameof(ParentViewModel))
                    return;

                UpdateIcon();

                var parent = ParentViewModel as MainWindowViewModel;
                if (parent != null)
                {
                    parent.PropertyChanged += (s, e) =>
                    {
                        if (e.PropertyName == nameof(MainWindowViewModel.SelectedMapping))
                            RaisePropertyChanged(nameof(IsDirectoryMapped));
                    };
                }
            };
        }

        private string? _fullName;

        public string? FullName
        {
            get => _fullName;
            set
            {
                if (!SetProperty(ref _fullName, value != null && !value.EndsWith(":\\") ? value.TrimEnd('\\') : value))
                    return;

                UpdateIcon();
            }
        }

        private string? _name;

        public string? Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private Icon? _icon;

        public Icon? Icon
        {
            get => _icon;
            set => SetProperty(ref _icon, value);
        }

        public void UpdateIcon()
        {
            if (_fullName == null)
            {
                Icon = null;
                return;
            }

            bool? isHighDpiContext = (ParentViewModel as MainWindowViewModel)?.IsHighDpiContext;

            Icon = isHighDpiContext.HasValue && isHighDpiContext.Value
                ? Win32.IconHelper.GetLargeIcon(_fullName)
                : Win32.IconHelper.GetSmallIcon(_fullName);
        }

        public bool IsDirectoryMapped
        {
            get => (ParentViewModel as MainWindowViewModel)?.SelectedMapping?.InitialDirectory == FullName;
        }

        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }
    }
}
