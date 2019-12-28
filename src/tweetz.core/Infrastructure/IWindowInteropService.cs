using System.Windows;
using tweetz.core.Models;

namespace tweetz.core.Infrastructure
{
    public interface IWindowInteropService
    {
        void SetWindowPosition(Window window, WindowPosition windowPosition);

        WindowPosition GetWindowPosition(Window window);

        void DisableMaximizeButton(Window window);

        void PowerManagmentRegistration(Window window, ISystemState systemState);
    }
}