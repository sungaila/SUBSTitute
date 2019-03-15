using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Sungaila.SUBSTitute.Core
{
    public interface IViewModelCommand
        : ICommand
    {
        void RaiseCanExecuteChanged();
    }
}
