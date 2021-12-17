using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Windows.Input;
using tweetz.core.Interfaces;
using tweetz.core.Services;
using twitter.core.Models;

namespace tweetz.core.Commands
{
    internal sealed class ShowTwitterStatusCommand : ICommandBinding
    {
        public static readonly RoutedCommand      Command = new RoutedUICommand();
        private                IMessageBoxService MessageBoxService { get; }

        public ShowTwitterStatusCommand(IMessageBoxService messageBoxService)
        {
            MessageBoxService = messageBoxService;
        }

        public CommandBinding CommandBinding()
        {
            return new CommandBinding(Command, CommandHandler);
        }

        [SuppressMessage("Usage", "VSTHRD100", MessageId = "Avoid async void methods")]
        private async void CommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                if (e.Parameter is TwitterStatus status)
                {
                    var options = new JsonSerializerOptions { WriteIndented = true };
                    var json    = JsonSerializer.Serialize(status, options);
                    await MessageBoxService.ShowMessageBoxAsync(json).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                TraceService.Message(ex.Message);
            }
        }
    }
}