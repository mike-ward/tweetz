using System.Collections.Generic;
using System.Collections.Specialized;

namespace tweetz.core.Models
{
    public class ObservableHashSet<T> : HashSet<T>, INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        public new bool Add(T item)
        {
            var added = base.Add(item);
            
            if (added)
            {
                var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new List<T> { item });
                CollectionChanged?.Invoke(this, eventArgs);
            }

            return added;
        }
    }
}