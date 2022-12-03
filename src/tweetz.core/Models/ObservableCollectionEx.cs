using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using tweetz.core.Services;

namespace tweetz.core.Models
{
    public class ObservableCollectionEx<T> : ObservableCollection<T>
    {
        public void AddNoNotify(T item)
        {
            Items.Add(item);
        }

        public void AddRange(IEnumerable<T> items)
        {
            var changed = false;

            foreach (var item in items)
            {
                Items.Add(item);
                changed = true;
            }

            if (changed)
            {
                NotifyCollectionChanged();
            }
        }

        public void InsertNoNotify(T item)
        {
            Items.Insert(0, item);
        }

        public void InsertRange(IEnumerable<T> items)
        {
            var changed = false;

            foreach (var item in items)
            {
                InsertNoNotify(item);
                changed = true;
            }

            if (changed)
            {
                NotifyCollectionChanged();
            }
        }

        public void RemoveAtNoNotify(int index)
        {
            Items.RemoveAt(index);
        }

        private Action<int>? debounceNotifyActionChanged;

        public void NotifyCollectionChanged()
        {
            debounceNotifyActionChanged ??= DebounceService.Debounce<int>(_ => SendNotifyCollectionChangedEvents());
            debounceNotifyActionChanged(0);
        }

        private void SendNotifyCollectionChangedEvents()
        {
            NotifyCollectionCountChanged();
            NotifyCollectionItemsChanged();
            NotifyCollectionReset();
        }

        private void NotifyCollectionReset()
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        private void NotifyCollectionCountChanged()
        {
            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
        }

        private void NotifyCollectionItemsChanged()
        {
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
        }
    }
}