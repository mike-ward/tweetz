using System.Text.Json;
using System.Windows.Input;
using tweetz.core.Infrastructure;
using twitter.core.Models;

namespace tweetz.core.Commands
{
    internal sealed class ShowTwitterStatusCommand : ICommandBinding
    {
        public static readonly RoutedCommand Command = new RoutedUICommand();
        private IMessageBoxService MessageBoxService { get; }

        public ShowTwitterStatusCommand(IMessageBoxService messageBoxService)
        {
            MessageBoxService = messageBoxService;
        }

        public CommandBinding CommandBinding()
        {
            return new CommandBinding(Command, CommandHandler);
        }

        private async void CommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter is TwitterStatus status)
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(status, options);
                await MessageBoxService.ShowMessageBoxAsync(json).ConfigureAwait(false);
            }
        }
    }
}