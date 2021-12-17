using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
using tweetz.core.Interfaces;
using tweetz.core.ViewModels;

namespace tweetz.core.Commands
{
    public class GetMentionsCommand : ICommandBinding
    {
        public static readonly RoutedCommand          Command = new RoutedUICommand();
        private                SearchControlViewModel SearchControlViewModel { get; }
        private                IMessageBoxService     MessageBoxService      { get; }

        private bool inCommand;

        public GetMentionsCommand(SearchControlViewModel searchControlViewModel, IMessageBoxService messageBoxService)
        {
            SearchControlViewModel = searchControlViewModel;
            MessageBoxService      = messageBoxService;
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
                if (inCommand) return;
                inCommand = true;
                await SearchControlViewModel.MentionsAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await MessageBoxService.ShowMessageBoxAsync(ex.Message).ConfigureAwait(false);
            }
            finally
            {
                inCommand = false;
            }
        }
    }
}