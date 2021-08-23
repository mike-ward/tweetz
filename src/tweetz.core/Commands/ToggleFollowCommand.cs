using System;
using System.Windows.Input;
using tweetz.core.Extensions;
using tweetz.core.Interfaces;
using twitter.core.Models;

namespace tweetz.core.Commands
{
    public class ToggleFollowCommand : ICommandBinding
    {
        public static readonly RoutedCommand Command = new RoutedUICommand();

        private ISettings          Settings          { get; }
        private ITwitterService    TwitterService    { get; }
        private IMessageBoxService MessageBoxService { get; }

        private bool inCommand;

        public ToggleFollowCommand(
            ISettings settings,
            ITwitterService twitterService,
            IMessageBoxService messageBoxService)
        {
            Settings          = settings;
            TwitterService    = twitterService;
            MessageBoxService = messageBoxService;
        }

        public CommandBinding CommandBinding()
        {
            return new CommandBinding(Command, CommandHandler, CanExecute);
        }

        private void CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = StatusFromParameter(e.Parameter) is not null || e.Parameter is User;
        }

        private async void CommandHandler(object sender, ExecutedRoutedEventArgs args)
        {
            try
            {
                if (inCommand) return;
                inCommand = true;

                var user = StatusFromParameter(args.Parameter)?.User ?? args.Parameter as User;
                if (user is not null)
                {
                    var screenName = user.ScreenName;
                    if (screenName is null) return;

                    if (user.IsFollowing)
                    {
                        await TwitterService.TwitterApi.Unfollow(screenName).ConfigureAwait(true);
                        user.Followers   = Math.Max(0, user.Followers - 1);
                        user.IsFollowing = false;
                    }
                    else
                    {
                        await TwitterService.TwitterApi.Follow(screenName).ConfigureAwait(true);
                        user.Followers++;
                        user.IsFollowing = true;
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