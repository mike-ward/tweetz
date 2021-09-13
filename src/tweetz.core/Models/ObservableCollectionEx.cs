using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

// ignore possible null references
#pragma warning disable 8602

namespace tweetz.core.Models
{
    public class ObservableCollectionEx<T> : ObservableCollection<T>
    {
        private readonly List<T>    internalList;
        private readonly MethodInfo countPropertyChanged;
        private readonly MethodInfo indexerPropertyChanged;
        private readonly MethodInfo collectionReset;

        public ObservableCollectionEx()
        {
            var itemsField = GetType().BaseType!.BaseType!.GetField("items", BindingFlags.NonPublic | BindingFlags.Instance);
            internalList = (List<T>)itemsField.GetValue(this)!;

            countPropertyChanged   = GetType().BaseType!.GetMethod("OnCountPropertyChanged", BindingFlags.NonPublic | BindingFlags.Instance)!;
            indexerPropertyChanged = GetType().BaseType!.GetMethod("OnIndexerPropertyChanged", BindingFlags.NonPublic | BindingFlags.Instance)!;
            collectionReset        = GetType().BaseType!.GetMethod("OnCollectionReset", BindingFlags.NonPublic | BindingFlags.Instance)!;
        }

        public void InsertNoNotify(T item)
        {
            internalList.Insert(0, item);
        }

        public void NotifyCollectionChanged()
        {
            countPropertyChanged.Invoke(this, null);
            indexerPropertyChanged.Invoke(this, null);
            collectionReset.Invoke(this, null);
        }
    }
}