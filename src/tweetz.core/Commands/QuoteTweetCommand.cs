using System.Windows.Input;
using tweetz.core.Infrastructure;
using tweetz.core.ViewModels;
using twitter.core.Models;

namespace tweetz.core.Commands
{
    public class QuoteTweetCommand : ICommandBinding
    {
        public static readonly RoutedCommand Command = new RoutedUICommand();
        private TabBarControlViewModel TabBarControlViewModel { get; }
        private ComposeControlViewModel ComposeControlViewModel { get; }

        public QuoteTweetCommand(TabBarControlViewModel tabBarControlViewModel, ComposeControlViewModel composeControlViewModel)
        {
            TabBarControlViewModel = tabBarControlViewModel;
            ComposeControlViewModel = composeControlViewModel;
        }

        public CommandBinding CommandBinding()
        {
            return new CommandBinding(Command, CommandHandler);
        }

        private void CommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (!(e.Parameter is TwitterStatus status) || status.Id == null || status.User == null)
            {
                return;
            }

            ComposeControlViewModel.Clear();
            ComposeControlViewModel.InReplyTo = status;
            ComposeControlViewModel.AttachmentUrl = status.StatusLink;
            TabBarControlViewModel.ShowComposeControl = true;
        }
    }
}