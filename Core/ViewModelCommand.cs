using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Sungaila.SUBSTitute.Core
{
    /// <summary>
    /// The base implementation of commands for view models.
    /// </summary>
    /// <typeparam name="TViewModel"></typeparam>
    public abstract class ViewModelCommand<TViewModel>
        : IViewModelCommand
        where TViewModel : ViewModel
    {
        public event EventHandler CanExecuteChanged;

        bool ICommand.CanExecute(object parameter)
        {
            TViewModel? viewModel = parameter as TViewModel;

            if (viewModel == null)
                return false;

            return CanExecute(viewModel);
        }

        void ICommand.Execute(object parameter)
        {
            TViewModel? viewModel = parameter as TViewModel;

            if (viewModel == null)
                return;

            Execute(viewModel);
        }

        public virtual bool CanExecute(TViewModel parameter)
        {
            return true;
        }

        public abstract void Execute(TViewModel parameter);

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
