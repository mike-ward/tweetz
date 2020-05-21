using System.Threading.Tasks;
using System.Windows;
using tweetz.core.Infrastructure;

namespace tweetz.core.Services
{
    public class MessageBoxService : IMessageBoxService
    {
        public void ShowMessageBox(string message)
        {
            var caption = (string)Application.Current.FindResource("title");
            MessageBox.Show(message, caption);
        }

        public MessageBoxResult ShowMessageBoxYesNo(string message)
        {
            var caption = (string)Application.Current.FindResource("title");
            return MessageBox.Show(message, caption, MessageBoxButton.YesNo);
        }

        public async ValueTask ShowMessageBoxAsync(string message)
        {
            await Task.Run(() => ShowMessageBox(message)).ConfigureAwait(true);
        }
    }
}