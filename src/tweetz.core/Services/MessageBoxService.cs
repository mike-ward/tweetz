using System.Threading.Tasks;
using System.Windows;
using tweetz.core.Infrastructure;

namespace tweetz.core.Services
{
    public class MessageBoxService : IMessageBoxService
    {
        public void ShowMessageBox(string message)
        {
            const string caption = "Tweetz";
            MessageBox.Show(message, caption);
        }

        public Task ShowMessageBoxAsync(string message)
        {
            return Task.Run(() => ShowMessageBox(message));
        }
    }
}