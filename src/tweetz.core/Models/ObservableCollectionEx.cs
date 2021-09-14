using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

// ignore possible null references
#pragma warning disable 8602

namespace tweetz.core.Models
{
    public class ObservableCollectionEx<T> : ObservableCollection<T>
    {
        private readonly IList<T>   itemsField;
        private readonly MethodInfo countPropertyChanged;
        private readonly MethodInfo indexerPropertyChanged;
        private readonly MethodInfo collectionReset;

        public ObservableCollectionEx()
        {
            var itemsFieldInfo = GetType().BaseType!.BaseType!.GetField("items", BindingFlags.NonPublic | BindingFlags.Instance);
            itemsField = (List<T>)itemsFieldInfo.GetValue(this)!;

            countPropertyChanged   = GetType().BaseType!.GetMethod("OnCountPropertyChanged", BindingFlags.NonPublic | BindingFlags.Instance)!;
            indexerPropertyChanged = GetType().BaseType!.GetMethod("OnIndexerPropertyChanged", BindingFlags.NonPublic | BindingFlags.Instance)!;
            collectionReset        = GetType().BaseType!.GetMethod("OnCollectionReset", BindingFlags.NonPublic | BindingFlags.Instance)!;
        }

        public void AddNoNotify(T item)
        {
            itemsField.Add(item);
        }

        public void AddRange(IEnumerable<T> items)
        {
            var notify = false;

            foreach (var item in items)
            {
                itemsField.Add(item);
                notify = true;
            }

            if (notify)
            {
                NotifyCollectionChanged();
            }
        }

        public void InsertNoNotify(T item)
        {
            itemsField.Insert(0, item);
        }

        public void InsertRange(IEnumerable<T> items)
        {
            var notify = false;

            foreach (var item in items)
            {
                InsertNoNotify(item);
                notify = true;
            }

            if (notify)
            {
                NotifyCollectionChanged();
            }
        }

        public void RemoveAtNoNotify(int index)
        {
            itemsField.RemoveAt(index);
        }

        public void NotifyCollectionChanged()
        {
            countPropertyChanged.Invoke(this, null);
            indexerPropertyChanged.Invoke(this, null);
            collectionReset.Invoke(this, null);
        }
    }
}