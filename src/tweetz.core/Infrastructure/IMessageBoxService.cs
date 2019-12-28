using System.Threading.Tasks;

namespace tweetz.core.Infrastructure
{
    public interface IMessageBoxService
    {
        void ShowMessageBox(string message);

        Task ShowMessageBoxAsync(string message);
    }
}