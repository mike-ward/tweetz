using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Input;
using tweetz.core.Infrastructure;
using tweetz.core.Services;
using tweetz.core.ViewModels;

namespace tweetz.core.Commands
{
    public class UpdateStatusCommand : ICommandBinding
    {
        public static readonly RoutedCommand Command = new RoutedUICommand();
        private ITwitterService TwitterService { get; }
        private ComposeControlViewModel ComposeControlViewModel { get; }
        private HomeTimelineControlViewModel HomeTimelineControlViewModel { get; }
        private TabBarControlViewModel TabBarControlViewModel { get; }
        private IMessageBoxService MessageBoxService { get; }

        public UpdateStatusCommand(
            ITwitterService twitterService,
            ComposeControlViewModel composeControlViewModel,
            HomeTimelineControlViewModel homeTimelineControlViewModel,
            TabBarControlViewModel tabBarControlViewModel,
            IMessageBoxService messageBoxService)
        {
            TwitterService = twitterService;
            ComposeControlViewModel = composeControlViewModel;
            HomeTimelineControlViewModel = homeTimelineControlViewModel;
            TabBarControlViewModel = tabBarControlViewModel;
            MessageBoxService = messageBoxService;
        }

        public CommandBinding CommandBinding()
        {
            return new CommandBinding(Command, CommandHandler, CanExecuteHandler);
        }

        private void CanExecuteHandler(object sender, CanExecuteRoutedEventArgs ea)
        {
            ea.CanExecute =
                !string.IsNullOrWhiteSpace(ComposeControlViewModel.StatusText) &&
                !ComposeControlViewModel.IsUpdating;
        }

        private async void CommandHandler(object sender, ExecutedRoutedEventArgs ea)
        {
            try
            {
                ComposeControlViewModel.IsUpdating = true;

                // AttachementUrl != null means tweet is being quoted (Retweet with comment).
                // Ignore InReplyTo.Id to register as a quoted tweet.
                // Twitter rules, not mine.
                var replyId = string.IsNullOrEmpty(ComposeControlViewModel.AttachmentUrl)
                    ? ComposeControlViewModel.InReplyTo?.Id
                    : null;

                var mediaIds = ComposeControlViewModel.Media
                    .Select(media => media.MediaId)
                    .ToArray();

                var statusText = ComposeControlViewModel.StatusText;
                var attachementUrl = ComposeControlViewModel.AttachmentUrl;

                var status = await TwitterService.UpdateStatus(
                    statusText,
                    replyId,
                    attachementUrl,
                    mediaIds)
                    .ConfigureAwait(true);

                TabBarControlViewModel.ShowComposeControl = false;
                ComposeControlViewModel.Clear();
                UpdateStatuses.Execute(new[] { status }, HomeTimelineControlViewModel);
            }
            catch (WebException ex)
            {
                var stream = ex.Response.GetResponseStream();
                using var reader = new StreamReader(stream);
                await MessageBoxService.ShowMessageBoxAsync(reader.ReadToEnd()).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await MessageBoxService.ShowMessageBoxAsync(ex.Message).ConfigureAwait(false);
            }
            finally
            {
                ComposeControlViewModel.IsUpdating = false;
            }
        }
    }
}