using System.Windows.Input;
using tweetz.core.Infrastructure;
using tweetz.core.ViewModels;
using twitter.core.Models;

namespace tweetz.core.Commands
{
    public class ReplyToCommand : ICommandBinding
    {
        public static readonly RoutedCommand Command = new RoutedUICommand();

        private ISettings Settings { get; }
        private TabBarControlViewModel TabBarControlViewModel { get; }
        private ComposeControlViewModel ComposeControlViewModel { get; }

        public ReplyToCommand(
            ISettings settings,
            TabBarControlViewModel tabBarControlViewModel, ComposeControlViewModel composeControlViewModel)
        {
            Settings = settings;
            TabBarControlViewModel = tabBarControlViewModel;
            ComposeControlViewModel = composeControlViewModel;
        }

        public CommandBinding CommandBinding()
        {
            return new CommandBinding(Command, CommandHandler, CanExecute);
        }

        private void CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = e.Parameter is TwitterStatus twitterStatus && twitterStatus.OriginatingStatus.User.ScreenName != Settings.ScreenName;
        }

        private void CommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (!(e.Parameter is TwitterStatus status) || status.Id == null || status.User == null)
            {
                return;
            }

            ComposeControlViewModel.Clear();
            ComposeControlViewModel.InReplyTo = status;
            ComposeControlViewModel.StatusText = $"@{status.User.ScreenName} ";
            TabBarControlViewModel.ShowComposeControl = true;
        }
    }
}