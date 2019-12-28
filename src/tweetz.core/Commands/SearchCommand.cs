using System.Windows.Input;
using tweetz.core.Infrastructure;
using tweetz.core.ViewModels;

namespace tweetz.core.Commands
{
    public class SearchCommand : ICommandBinding
    {
        public static readonly RoutedCommand Command = new RoutedUICommand();
        private SearchControlViewModel SearchControlViewModel { get; }
        private bool inCommand;

        public SearchCommand(SearchControlViewModel searchControlViewModel)
        {
            SearchControlViewModel = searchControlViewModel;
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
                    await SearchControlViewModel.Search(query);
                }
            }
            finally
            {
                inCommand = false;
            }
        }
    }
}