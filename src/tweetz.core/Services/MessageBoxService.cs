using System.Threading.Tasks;
using System.Windows;
using tweetz.core.Infrastructure;

namespace tweetz.core.Services
{
    public class MessageBoxService : IMessageBoxService
    {
        public void ShowMessageBox(string message)
        {
            MessageBox.Show(message);
        }

        public async Task ShowMessageBoxAsync(string message)
        {
            await Task.Run(() => ShowMessageBox(message)).ConfigureAwait(true);
        }
    }
}