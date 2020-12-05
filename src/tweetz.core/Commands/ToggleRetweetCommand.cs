using System;
using System.Windows.Input;
using tweetz.core.Extensions;
using tweetz.core.Interfaces;
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
            e.CanExecute = StatusFromParameter(e.Parameter) is not null;
        }

        private async void CommandHandler(object sender, ExecutedRoutedEventArgs args)
        {
            try
            {
                if (inCommand) return;
                inCommand = true;
                var twitterStatus = StatusFromParameter(args.Parameter);
                if (twitterStatus is not null)
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
                && twitterStatus.OriginatingStatus.User.ScreenName.IsNotEqualTo(Settings.ScreenName)
                && !twitterStatus.IsMyTweet
                    ? twitterStatus
                    : null;
        }
    }
}