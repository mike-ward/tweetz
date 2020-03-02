using System;
using System.Threading.Tasks;
using System.Windows.Input;
using tweetz.core.Infrastructure;
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

        private void CommandHandler(object sender, ExecutedRoutedEventArgs args)
        {
            CommandHandlerAsync(args).ConfigureAwait(false);
        }

        private async Task CommandHandlerAsync(ExecutedRoutedEventArgs args)
        {
            if (inCommand) return;

            try
            {
                inCommand = true;

                if (args.Parameter is string query)
                {
                    await SearchControlViewModel.Search(query).ConfigureAwait(false);
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