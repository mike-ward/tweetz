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
            return new CommandBinding(Command, CommandHandler, CanExecute);
        }

        private static void CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = StatusFromParameter(e.Parameter) is not null;
        }

        private void CommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            var status = StatusFromParameter(e.Parameter);
            if (status is null) return;

            ComposeControlViewModel.Clear();
            ComposeControlViewModel.InReplyTo = status;
            ComposeControlViewModel.AttachmentUrl = status.StatusLink;
            TabBarControlViewModel.ShowComposeControl = true;
        }

        private static TwitterStatus? StatusFromParameter(object parameter)
        {
            return parameter is TwitterStatus twitterStatus && twitterStatus.Id is not null && twitterStatus.User is not null
                ? twitterStatus
                : null;
        }
    }
}