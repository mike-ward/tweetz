using System.Threading.Tasks;
using System.Windows;
using tweetz.core.Interfaces;

namespace tweetz.core.Services
{
    public class MessageBoxService : IMessageBoxService
    {
        public void ShowMessageBox(string message)
        {
            var caption = Application.Current.FindResource("title") as string ?? string.Empty;
            MessageBox.Show(message, caption);
        }

        public MessageBoxResult ShowMessageBoxYesNo(string message)
        {
            var caption = Application.Current.FindResource("title") as string ?? string.Empty;
            return MessageBox.Show(message, caption, MessageBoxButton.YesNo);
        }

        public async ValueTask ShowMessageBoxAsync(string message)
        {
            await Task.Run(() => ShowMessageBox(message)).ConfigureAwait(true);
        }
    }
}