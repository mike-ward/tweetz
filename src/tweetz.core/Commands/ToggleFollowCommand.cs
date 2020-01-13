using System;
using System.Windows.Input;
using tweetz.core.Infrastructure;
using twitter.core.Models;

namespace tweetz.core.Commands
{
    public class ToggleFollowCommand : ICommandBinding
    {
        public static readonly RoutedCommand Command = new RoutedUICommand();

        private ISettings Settings { get; }
        private ITwitterService TwitterService { get; }
        private IMessageBoxService MessageBoxService { get; }

        private bool inCommand;

        public ToggleFollowCommand(
            ISettings settings,
            ITwitterService twitterService,
            IMessageBoxService messageBoxService)
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
            e.CanExecute = e.Parameter is TwitterStatus twitterStatus && twitterStatus.OriginatingStatus.User.ScreenName != Settings.ScreenName;
        }

        private async void CommandHandler(object sender, ExecutedRoutedEventArgs args)
        {
            if (inCommand) return;

            try
            {
                inCommand = true;

                if (args.Parameter is TwitterStatus twitterStatus)
                {
                    if (twitterStatus.IsMyTweet) return;
                    if (twitterStatus.User == null) return;
                    var screenName = twitterStatus.User.ScreenName;
                    if (screenName == null) return;

                    if (twitterStatus.User.IsFollowing)
                    {
                        await TwitterService.Unfollow(screenName);
                        var user = twitterStatus.User;
                        if (user != null)
                        {
                            user.Followers = Math.Max(0, user.Followers - 1);
                            user.IsFollowing = false;
                        }
                    }
                    else
                    {
                        await TwitterService.Follow(screenName);
                        var user = twitterStatus.User;
                        if (user != null)
                        {
                            user.Followers += 1;
                            user.IsFollowing = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await MessageBoxService.ShowMessageBoxAsync(ex.Message);
            }
            finally
            {
                inCommand = false;
            }
        }
    }
}