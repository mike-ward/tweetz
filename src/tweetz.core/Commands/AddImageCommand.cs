using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using tweetz.core.Infrastructure;
using tweetz.core.Models;
using tweetz.core.ViewModels;
using twitter.core.Models;

namespace tweetz.core.Commands
{
    public class AddImageCommand : ICommandBinding
    {
        public static readonly RoutedCommand Command = new RoutedUICommand();
        private ComposeControlViewModel ComposeControlViewModel { get; }
        public ITwitterService TwitterService { get; }
        public IMessageBoxService MessageBoxService { get; }

        public AddImageCommand(ComposeControlViewModel composeControlViewModel, ITwitterService twitterService, IMessageBoxService messageBoxService)
        {
            ComposeControlViewModel = composeControlViewModel;
            TwitterService = twitterService;
            MessageBoxService = messageBoxService;
        }

        public CommandBinding CommandBinding()
        {
            return new CommandBinding(Command, CommandHandler, CanExecute);
        }

        private void CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ComposeControlViewModel.CanAddImage();
        }

        private async void CommandHandler(object sender, ExecutedRoutedEventArgs ea)
        {
            await CommandHandlerAsync().ConfigureAwait(false);
        }

        private async ValueTask CommandHandlerAsync()
        {
            const string filter = "Image files (*.gif;*.jpg;*.png;*.webp;*.mp4)|*.gif;*.jpg;*.png;*.webp;*.mp4";

            using var ofd = new OpenFileDialog
            {
                Filter = filter,
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ComposeControlViewModel.IsUploadingMedia = true;
                    var mediaInfo = await UploadMediaAsync(ofd.FileName).ConfigureAwait(true);
                    ComposeControlViewModel.Media.Add(mediaInfo);
                }
                catch (WebException ex)
                {
                    var stream = ex.Response?.GetResponseStream();
                    if (stream is null) { return; }
                    using var reader = new StreamReader(stream);
                    var message = await reader.ReadToEndAsync().ConfigureAwait(false);
                    await MessageBoxService.ShowMessageBoxAsync(message).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    await MessageBoxService.ShowMessageBoxAsync(ex.Message).ConfigureAwait(false);
                }
                finally
                {
                    ComposeControlViewModel.IsUploadingMedia = false;
                }
            }
        }

        private async ValueTask<MediaInfo> UploadMediaAsync(string path)
        {
            var contentType = ComposeControlViewModel.ContentType(path);
            var mediaId = await UploadAsync(path, contentType).ConfigureAwait(false);
            return new MediaInfo { Path = path, MediaId = mediaId };
        }

        private async ValueTask<string> UploadAsync(string filename, string mediaType)
        {
            var bytes = await File.ReadAllBytesAsync(filename).ConfigureAwait(false);
            var media = await TwitterService.UploadMediaInit(bytes.Length, mediaType).ConfigureAwait(false);
            await TwitterService.UploadMediaAppend(media.MediaId, 0, bytes).ConfigureAwait(false);
            var finalize = await TwitterService.UploadMediaFinalize(media.MediaId).ConfigureAwait(false);

            if (finalize.ProcessingInfo != null)
            {
                await UntilProcessingFinishedAsync(media.MediaId).ConfigureAwait(false);
            }

            return media.MediaId;
        }

        private async ValueTask UntilProcessingFinishedAsync(string mediaId)
        {
            while (true)
            {
                var status = await TwitterService.UploadMediaStatus(mediaId).ConfigureAwait(false);
                if (string.CompareOrdinal(status.ProcessingInfo.State, ProcessingInfo.StateSucceeded) == 0) break;
                var milliseconds = (int)TimeSpan.FromSeconds(status.ProcessingInfo.CheckAfterSecs).TotalMilliseconds;
                await Task.Delay(milliseconds).ConfigureAwait(false);
            }
        }
    }
}