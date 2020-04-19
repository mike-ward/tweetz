using System.Threading.Tasks;
using System.Windows;

namespace tweetz.core.Infrastructure
{
    public interface IMessageBoxService
    {
        void ShowMessageBox(string message);

        MessageBoxResult ShowMessageBoxYesNo(string message);

        ValueTask ShowMessageBoxAsync(string message);
    }
}