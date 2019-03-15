using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Sungaila.SUBSTitute.Core
{
    public class ObservableViewModelCollection<TViewModel>
        : ObservableCollection<TViewModel>
        where TViewModel : ViewModel
    {
        private ViewModel? OwnerViewModel { get; }

        public ObservableViewModelCollection(ViewModel viewModel)
        {
            OwnerViewModel = viewModel;
            CollectionChanged += ObservableViewModelCollection_CollectionChanged;
        }

        private void ObservableViewModelCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (TViewModel item in e.OldItems.Cast<TViewModel>())
                    item.ParentViewModel = null;
            }

            if (e.NewItems != null)
            {
                foreach (TViewModel item in e.NewItems.Cast<TViewModel>())
                    item.ParentViewModel = OwnerViewModel;
            }
        }

        public void Observe(Action action, params string[] propertyNames)
        {
            if (action == null || propertyNames == null)
                return;

            PropertyChangedEventHandler propertyChangedHandler = (sender, e) =>
            {
                if (!propertyNames.Contains(e.PropertyName))
                    return;

                action.Invoke();
            };

            NotifyCollectionChangedEventHandler collectionChangedHandler = (sender, e) =>
            {
                if (e.OldItems != null)
                {
                    foreach (var item in e.OldItems.Cast<ViewModel>())
                        item.PropertyChanged -= propertyChangedHandler;
                }

                if (e.NewItems != null)
                {
                    foreach (var item in e.NewItems.Cast<ViewModel>())
                        item.PropertyChanged += propertyChangedHandler;
                }
            };

            CollectionChanged -= collectionChangedHandler;
            CollectionChanged += collectionChangedHandler;

            foreach (ViewModel item in this)
            {
                item.PropertyChanged -= propertyChangedHandler;
                item.PropertyChanged += propertyChangedHandler;
            }
        }
    }
}
