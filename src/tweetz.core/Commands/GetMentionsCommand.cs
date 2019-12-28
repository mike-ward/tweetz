using System.Windows.Input;
using tweetz.core.Infrastructure;
using tweetz.core.ViewModels;

namespace tweetz.core.Commands
{
    public class GetMentionsCommand : ICommandBinding
    {
        public static readonly RoutedCommand Command = new RoutedUICommand();
        public SearchControlViewModel SearchControlViewModel { get; }
        private bool inCommand;

        public GetMentionsCommand(SearchControlViewModel searchControlViewModel)
        {
            SearchControlViewModel = searchControlViewModel;
        }

        public CommandBinding CommandBinding()
        {
            return new CommandBinding(Command, CommandHandler);
        }

        private async void CommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (inCommand) return;

            try
            {
                inCommand = true;
                await SearchControlViewModel.Mentions();
            }
            finally
            {
                inCommand = false;
            }
        }
    }
}