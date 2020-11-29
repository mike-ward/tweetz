using System.ComponentModel;

namespace tweetz.core.Interfaces
{
    public interface ICheckForUpdates : INotifyPropertyChanged
    {
        string Version { get; }
    }
}