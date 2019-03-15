using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace Sungaila.SUBSTitute.Core
{
    /// <summary>
    /// The base implementation of every ViewModel.
    /// </summary>
    public abstract class ViewModel
        : INotifyPropertyChanged
    {
        /// <summary>
        /// Implementation of <see cref="INotifyPropertyChanged.PropertyChanged"/>.
        /// Is used to support bindings between views and view model properties.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event for the given property name.
        /// </summary>
        /// <param name="propertyName">The name of the property which has been changed. When omitted the property name will be the member name of the caller (which is ideally the view model property).</param>
        protected void RaisePropertyChanged([CallerMemberName]string? propertyName = null)
        {
            if (String.IsNullOrEmpty(propertyName))
            {
                Debug.Fail($"{nameof(ViewModel)}.{nameof(ViewModel.RaisePropertyChanged)} has been called with a null or empty {nameof(propertyName)}.");
                return;
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Checks if the old value and new value differ.
        /// If both values are unequal, then the new value is set and <see cref="RaisePropertyChanged"/> is called.
        /// Call this method whenever a view model property has changed (and bound views must notice).
        /// </summary>
        /// <typeparam name="T">The type of the changed property.</typeparam>
        /// <param name="propertyField">The property field which contains the old value.</param>
        /// <param name="newValue">The new value to set.</param>
        /// <param name="propertyName">The name of the property which has been changed. When omitted the property name will be the member name of the caller (which is ideally the view model property).</param>
        /// <returns>Returns <see cref="true"/> when the value was set.</returns>
        protected bool SetProperty<T>(ref T propertyField, T newValue, [CallerMemberName]string? propertyName = null)
        {
            if (String.IsNullOrEmpty(propertyName))
            {
                Debug.Fail($"{nameof(ViewModel)}.{nameof(SetProperty)} has been called with a null or empty {nameof(propertyName)}.");
                return false;
            }

            if (EqualityComparer<T>.Default.Equals(propertyField, newValue))
                return false;

            propertyField = newValue;
            RaisePropertyChanged(propertyName);

            return true;
        }

        /// <summary>
        /// Functions just like <see cref="SetProperty"/> but ensures that <see cref="ParentViewModel"/> are set properly.
        /// </summary>
        /// <typeparam name="TViewModel">The view model type of the changed property.</typeparam>
        /// <param name="propertyField">The property field which contains the old value.</param>
        /// <param name="newValue">The new value to set.</param>
        /// <param name="propertyName">The name of the property which has been changed. When omitted the property name will be the member name of the caller (which is ideally the view model property).</param>
        /// <returns>Returns <see cref="true"/> when the value was set.</returns>
        protected bool SetViewModelProperty<TViewModel>(ref TViewModel propertyField, TViewModel newValue, [CallerMemberName]string? propertyName = null)
            where TViewModel : ViewModel?
        {
            if (!SetProperty(ref propertyField, newValue, propertyName))
                return false;

            if(newValue != null)
                newValue.ParentViewModel = this;

            return true;
        }

        private ViewModel? _parentViewModel;

        /// <summary>
        /// The logical parent of this view model.
        /// </summary>
        public ViewModel? ParentViewModel
        {
            get => _parentViewModel;
            set => SetProperty(ref _parentViewModel, value);
        }

        /// <summary>
        /// The top most parent of this view model.
        /// </summary>
        public ViewModel? RootViewModel
        {
            get
            {
                ViewModel? parent = ParentViewModel;

                while (parent != null)
                {
                    if (parent.ParentViewModel == null)
                        return parent;

                    parent = parent.ParentViewModel;
                }

                return null;
            }
        }

        /// <summary>
        /// Adds commands to this view model.
        /// This ensures that <see cref="ICommand.CanExecute(object)"/> is called whenever a property was changed.
        /// </summary>
        /// <param name="commands"></param>
        protected void AddCommands(params IViewModelCommand[] commands)
        {
            foreach (IViewModelCommand command in commands)
            {
                if (command == null || Commands.ContainsKey(command.GetType().FullName))
                    continue;

                Commands.Add(command.GetType().FullName, command);
                PropertyChanged += (sender, e) => command.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Removes existing commands for this view model.
        /// </summary>
        /// <param name="commands"></param>
        protected void RemoveCommands(params IViewModelCommand[] commands)
        {
            foreach (IViewModelCommand command in commands)
            {
                if (command == null || !Commands.ContainsKey(command.GetType().FullName))
                    continue;

                Commands.Remove(command.GetType().FullName);
                PropertyChanged -= (sender, e) => command.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// A dictionary filled with commands for this view model. The key is the <see cref="Type.FullName"/> of the command.
        /// </summary>
        public Dictionary<string, IViewModelCommand> Commands { get; } = new Dictionary<string, IViewModelCommand>();
    }
}
