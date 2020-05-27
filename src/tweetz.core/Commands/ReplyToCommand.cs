using System.Globalization;
using System.Windows;
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
            TabBarControlViewModel tabBarControlViewModel,
            ComposeControlViewModel composeControlViewModel)
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
            e.CanExecute = StatusFromParameter(e.Parameter) != null;
        }

        private void CommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            var status = StatusFromParameter(e.Parameter);
            if (status is null) return;

            ComposeControlViewModel.Clear();
            ComposeControlViewModel.InReplyTo = status;
            var watermarkFormat = (string)Application.Current.FindResource("in-reply-to");
            ComposeControlViewModel.WatermarkText = string.Format(CultureInfo.InvariantCulture, watermarkFormat, status.User.ScreenName);
            TabBarControlViewModel.ShowComposeControl = true;
        }

        private TwitterStatus? StatusFromParameter(object parameter)
        {
            return
                parameter is TwitterStatus twitterStatus
                && string.CompareOrdinal(twitterStatus.OriginatingStatus.User.ScreenName, Settings.ScreenName) != 0
                && !twitterStatus.IsMyTweet
                    ? twitterStatus
                    : null;
        }
    }
}