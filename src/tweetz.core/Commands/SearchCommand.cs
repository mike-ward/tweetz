using System;
using System.Windows.Input;
using tweetz.core.Interfaces;
using tweetz.core.ViewModels;

namespace tweetz.core.Commands
{
    public class SearchCommand : ICommandBinding
    {
        public static readonly RoutedCommand Command = new RoutedUICommand();
        private SearchControlViewModel SearchControlViewModel { get; }
        public IMessageBoxService MessageBoxService { get; }

        private bool inCommand;

        public SearchCommand(SearchControlViewModel searchControlViewModel, IMessageBoxService messageBoxService)
        {
            SearchControlViewModel = searchControlViewModel;
            MessageBoxService = messageBoxService;
        }

        public CommandBinding CommandBinding()
        {
            return new CommandBinding(Command, CommandHandler);
        }

        private async void CommandHandler(object sender, ExecutedRoutedEventArgs args)
        {
            if (inCommand) return;

            try
            {
                inCommand = true;

                if (args.Parameter is string query)
                {
                    await SearchControlViewModel.SearchAsync(query).ConfigureAwait(false);
                }
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