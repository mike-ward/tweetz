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
        private readonly MethodInfo onCountPropertyChanged;
        private readonly MethodInfo OnIndexerPropertyChanged;
        private readonly MethodInfo OnCollectionReset;

        public ObservableCollectionEx()
        {
            var itemsField = GetType().BaseType!.BaseType!.GetField("items", BindingFlags.NonPublic | BindingFlags.Instance);
            internalList = (List<T>)itemsField.GetValue(this)!;

            onCountPropertyChanged   = GetType().BaseType!.GetMethod("OnCountPropertyChanged", BindingFlags.NonPublic | BindingFlags.Instance)!;
            OnIndexerPropertyChanged = GetType().BaseType!.GetMethod("OnIndexerPropertyChanged", BindingFlags.NonPublic | BindingFlags.Instance)!;
            OnCollectionReset        = GetType().BaseType!.GetMethod("OnCollectionReset", BindingFlags.NonPublic | BindingFlags.Instance)!;
        }

        public void InsertNoNotify(T item)
        {
            internalList.Insert(0, item);
        }

        public void NotifyCollectionChanged()
        {
            onCountPropertyChanged.Invoke(this, null);
            OnIndexerPropertyChanged.Invoke(this, null);
            OnCollectionReset.Invoke(this, null);
        }
    }
}