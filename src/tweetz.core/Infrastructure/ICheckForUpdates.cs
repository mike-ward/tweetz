using System.ComponentModel;

namespace tweetz.core.Infrastructure
{
    public interface ICheckForUpdates : INotifyPropertyChanged
    {
        string Version { get; }
    }
}