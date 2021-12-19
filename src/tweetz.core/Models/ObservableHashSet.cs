﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace tweetz.core.Models
{
    [Serializable]
    public class ObservableHashSet<T> : HashSet<T>, INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        public ObservableHashSet() { }

        protected ObservableHashSet(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

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

        public new void Clear()
        {
            if (Count > 0)
            {
                base.Clear();
                var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
                CollectionChanged?.Invoke(this, eventArgs);
            }
        }
    }
}