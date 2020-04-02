using System.Windows;

namespace tweetz.core.Infrastructure
{
    public interface ISystemTrayIconService
    {
        void HideSystemTrayIcon();

        void InitializeSystemTrayIcon(Window window);
    }
}