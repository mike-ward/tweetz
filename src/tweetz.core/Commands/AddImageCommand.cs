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

        private void CommandHandler(object sender, ExecutedRoutedEventArgs ea)
        {
            CommandHandlerAsync().ConfigureAwait(false);
        }

        private async Task CommandHandlerAsync()
        {
            const string filter = "Image files (*.gif;*.jpg;*.png;*.webp;*.mp4)|*.gif;*.jpg;*.png;*.webp;*.mp4";

            using var ofd = new OpenFileDialog
            {
                Filter = filter
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ComposeControlViewModel.IsUploadingMedia = true;
                    var mediaInfo = await UploadMedia(ofd.FileName);
                    ComposeControlViewModel.Media.Add(mediaInfo);
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
                    ComposeControlViewModel.IsUploadingMedia = false;
                }
            }
        }

        private async Task<MediaInfo> UploadMedia(string path)
        {
            var contentType = ComposeControlViewModel.ContentType(path);
            var mediaId = await Upload(path, contentType).ConfigureAwait(false);
            return new MediaInfo { Path = path, MediaId = mediaId };
        }

        private async Task<string> Upload(string filename, string mediaType)
        {
            var bytes = File.ReadAllBytes(filename);
            var media = await TwitterService.UploadMediaInit(bytes.Length, mediaType);
            await TwitterService.UploadMediaAppend(media.MediaId, 0, bytes);
            var finalize = await TwitterService.UploadMediaFinalize(media.MediaId);

            if (finalize.ProcessingInfo != null)
            {
                await UntilProcessingFinished(media.MediaId).ConfigureAwait(false);
            }

            return media.MediaId;
        }

        private async Task UntilProcessingFinished(string mediaId)
        {
            while (true)
            {
                var status = await TwitterService.UploadMediaStatus(mediaId).ConfigureAwait(false);
                if (status.ProcessingInfo.State == ProcessingInfo.StateSuceedded) break;
                var milliseconds = (int)TimeSpan.FromSeconds(status.ProcessingInfo.CheckAfterSecs).TotalMilliseconds;
                await Task.Delay(milliseconds).ConfigureAwait(false);
            }
        }
    }
}