using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace tweetz.core.Infrastructure
{
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void SetProperty<T>(ref T item, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(item, value)) return;
            item = value;
            OnPropertyChanged(propertyName);
        }

        protected virtual void OnPropertyChanged(string? propertyName)
        {
            if (propertyName is null) return;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}