using System;
using System.Windows.Input;
using tweetz.core.Extensions;
using tweetz.core.Interfaces;
using tweetz.core.ViewModels;
using twitter.core.Models;

namespace tweetz.core.Commands
{
    public class ToggleFavoriteCommand : ICommandBinding
    {
        public static readonly RoutedCommand Command = new RoutedUICommand();

        private ISettings                         Settings                          { get; }
        private ITwitterService                   TwitterService                    { get; }
        private IMessageBoxService                MessageBoxService                 { get; }
        private FavoritesTimelineControlViewModel FavoritesTimelineControlViewModel { get; }

        private bool inCommand;

        public ToggleFavoriteCommand(
            ISettings settings,
            ITwitterService twitterService,
            IMessageBoxService messageBoxService,
            FavoritesTimelineControlViewModel favoritesTimelineControlViewModel)
        {
            Settings                          = settings;
            TwitterService                    = twitterService;
            MessageBoxService                 = messageBoxService;
            FavoritesTimelineControlViewModel = favoritesTimelineControlViewModel;
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
            if (inCommand) return;

            try
            {
                inCommand = true;
                var twitterStatus = StatusFromParameter(args.Parameter);
                if (twitterStatus is not null)
                {
                    if (twitterStatus.IsMyTweet) return;

                    if (twitterStatus.Favorited)
                    {
                        await TwitterService.DestroyFavorite(twitterStatus.Id).ConfigureAwait(true);
                        twitterStatus.FavoriteCount = Math.Max(0, twitterStatus.FavoriteCount - 1);
                        twitterStatus.Favorited     = false;
                    }
                    else
                    {
                        await TwitterService.CreateFavorite(twitterStatus.Id).ConfigureAwait(true);
                        twitterStatus.FavoriteCount++;
                        twitterStatus.Favorited = true;
                    }

                    await FavoritesTimelineControlViewModel.UpdateAsync().ConfigureAwait(false);
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