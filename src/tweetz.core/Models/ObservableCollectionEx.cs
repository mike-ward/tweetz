using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

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

        public void NotifyCollectionChanged()
        {
            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}