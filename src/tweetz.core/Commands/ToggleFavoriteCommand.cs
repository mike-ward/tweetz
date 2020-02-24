using System;
using System.Windows.Input;
using tweetz.core.Infrastructure;
using tweetz.core.ViewModels;
using twitter.core.Models;

namespace tweetz.core.Commands
{
    public class ToggleFavoritesCommand : ICommandBinding
    {
        public static readonly RoutedCommand Command = new RoutedUICommand();

        private ISettings Settings { get; }
        private ITwitterService TwitterService { get; }
        private IMessageBoxService MessageBoxService { get; }
        private FavoritesTimelineControlViewModel FavoritesTimelineControlViewModel { get; }

        private bool inCommand;

        public ToggleFavoritesCommand(
            ISettings settings,
            ITwitterService twitterService,
            IMessageBoxService messageBoxService,
            FavoritesTimelineControlViewModel favoritesTimelineControlViewModel)
        {
            Settings = settings;
            TwitterService = twitterService;
            MessageBoxService = messageBoxService;
            FavoritesTimelineControlViewModel = favoritesTimelineControlViewModel;
        }

        public CommandBinding CommandBinding()
        {
            return new CommandBinding(Command, CommandHandler, CanExecute);
        }

        private void CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = StatusFromParameter(e.Parameter) != null;
        }

        private async void CommandHandler(object sender, ExecutedRoutedEventArgs args)
        {
            if (inCommand) return;

            try
            {
                inCommand = true;
                var twitterStatus = StatusFromParameter(args.Parameter);
                if (twitterStatus != null)
                {
                    if (twitterStatus.IsMyTweet) return;

                    if (twitterStatus.Favorited)
                    {
                        await TwitterService.DestroyFavorite(twitterStatus.Id);
                        twitterStatus.FavoriteCount = Math.Max(0, twitterStatus.FavoriteCount - 1);
                        twitterStatus.Favorited = false;
                    }
                    else
                    {
                        await TwitterService.CreateFavorite(twitterStatus.Id);
                        twitterStatus.FavoriteCount += 1;
                        twitterStatus.Favorited = true;
                    }

                    await FavoritesTimelineControlViewModel.Update();
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

        private TwitterStatus? StatusFromParameter(object parameter)
        {
            return
                parameter is TwitterStatus twitterStatus
                && twitterStatus.OriginatingStatus.User.ScreenName != Settings.ScreenName
                && !twitterStatus.IsMyTweet
                    ? twitterStatus
                    : null;
        }
    }
}