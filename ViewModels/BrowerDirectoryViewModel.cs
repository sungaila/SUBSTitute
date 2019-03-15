using Sungaila.SUBSTitute.Command;
using Sungaila.SUBSTitute.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace Sungaila.SUBSTitute.ViewModels
{
    [DebuggerDisplay("BrowerDirectoryViewModel {Name}")]
    public class BrowerDirectoryViewModel
        : ViewModel
    {
        public BrowerDirectoryViewModel()
        {
            PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(ParentViewModel))
                    UpdateIcon();
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
    }
}
