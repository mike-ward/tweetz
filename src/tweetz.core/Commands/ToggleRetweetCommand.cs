using System;
using System.Threading.Tasks;
using System.Windows.Input;
using tweetz.core.Infrastructure;
using twitter.core.Models;

namespace tweetz.core.Commands
{
    public class ToggleRetweetCommand : ICommandBinding
    {
        public static readonly RoutedCommand Command = new RoutedUICommand();

        private ISettings Settings { get; }
        private ITwitterService TwitterService { get; }
        private IMessageBoxService MessageBoxService { get; }

        private bool inCommand;

        public ToggleRetweetCommand(
            ISettings settings,
            ITwitterService twitterService, IMessageBoxService messageBoxService)
        {
            Settings = settings;
            TwitterService = twitterService;
            MessageBoxService = messageBoxService;
        }

        public CommandBinding CommandBinding()
        {
            return new CommandBinding(Command, CommandHandler, CanExecute);
        }

        private void CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = StatusFromParameter(e.Parameter) != null;
        }

        private void CommandHandler(object sender, ExecutedRoutedEventArgs args)
        {
            CommandHandlerAsync(args).ConfigureAwait(false);
        }

        private async ValueTask CommandHandlerAsync(ExecutedRoutedEventArgs args)
        {
            try
            {
                if (inCommand) return;
                inCommand = true;
                var twitterStatus = StatusFromParameter(args.Parameter);
                if (twitterStatus != null)
                {
                    if (twitterStatus.IsMyTweet) return;

                    if (twitterStatus.RetweetedByMe)
                    {
                        await TwitterService.UnretweetStatus(twitterStatus.Id).ConfigureAwait(true);
                        twitterStatus.RetweetCount = Math.Max(0, twitterStatus.RetweetCount - 1);
                        twitterStatus.RetweetedByMe = false;
                    }
                    else
                    {
                        await TwitterService.RetweetStatus(twitterStatus.Id).ConfigureAwait(true);
                        twitterStatus.RetweetCount++;
                        twitterStatus.RetweetedByMe = true;
                    }
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