using System.Windows;

namespace tweetz.core.Interfaces
{
    public interface ISystemTrayIconService
    {
        void Close();

        void Initialize(Window window);
    }
}