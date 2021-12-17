using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Windows;
using tweetz.core.Interfaces;

namespace tweetz.core.Services
{
    public class MessageBoxService : IMessageBoxService
    {
        [SuppressMessage("Usage", "VSTHRD001", MessageId = "Avoid legacy thread switching APIs")]
        public void ShowMessageBox(string message)
        {
            var caption = App.GetString("title");
            Application.Current.Dispatcher.Invoke(() => MessageBox.Show(Application.Current.MainWindow!, message, caption, MessageBoxButton.OK, MessageBoxImage.Information));
        }

        public MessageBoxResult ShowMessageBoxYesNo(string message)
        {
            var caption = App.GetString("title");
            return MessageBox.Show(Application.Current.MainWindow!, message, caption, MessageBoxButton.YesNo, MessageBoxImage.Question);
        }

        public async ValueTask ShowMessageBoxAsync(string message)
        {
            await Task.Run(() => ShowMessageBox(message)).ConfigureAwait(true);
        }
    }
}