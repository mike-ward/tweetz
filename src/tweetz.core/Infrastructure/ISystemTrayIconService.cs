using System.Windows;

namespace tweetz.core.Infrastructure
{
    public interface ISystemTrayIconService
    {
        void Close();

        void Initialize(Window window);
    }
}