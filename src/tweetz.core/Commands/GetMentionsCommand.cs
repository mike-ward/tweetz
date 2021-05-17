using System;
using System.Windows.Input;
using tweetz.core.Interfaces;
using tweetz.core.ViewModels;

namespace tweetz.core.Commands
{
    public class GetMentionsCommand : ICommandBinding
    {
        public static readonly RoutedCommand          Command = new RoutedUICommand();
        public                 SearchControlViewModel SearchControlViewModel { get; }
        public                 IMessageBoxService     MessageBoxService      { get; }

        private bool inCommand;

        public GetMentionsCommand(SearchControlViewModel searchControlViewModel, IMessageBoxService messageBoxService)
        {
            SearchControlViewModel = searchControlViewModel;
            MessageBoxService      = messageBoxService;
        }

        public CommandBinding CommandBinding()
        {
            return new(Command, CommandHandler);
        }

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