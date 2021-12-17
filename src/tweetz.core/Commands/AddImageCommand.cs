using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using tweetz.core.Extensions;
using tweetz.core.Interfaces;
using tweetz.core.Models;
using tweetz.core.Services;
using tweetz.core.ViewModels;
using twitter.core.Models;

namespace tweetz.core.Commands
{
    public class AddImageCommand : ICommandBinding
    {
        public static readonly RoutedCommand           Command = new RoutedUICommand();
        private                ComposeControlViewModel ComposeControlViewModel { get; }
        private                ITwitterService         TwitterService          { get; }
        private                IMessageBoxService      MessageBoxService       { get; }

        public AddImageCommand(ComposeControlViewModel composeControlViewModel, ITwitterService twitterService, IMessageBoxService messageBoxService)
        {
            ComposeControlViewModel = composeControlViewModel;
            TwitterService          = twitterService;
            MessageBoxService       = messageBoxService;
        }

        public CommandBinding CommandBinding()
        {
            return new CommandBinding(Command, CommandHandler, CanExecute);
        }

        private void CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ComposeControlViewModel.CanAddImage();
        }

        [SuppressMessage("Usage", "VSTHRD100", MessageId = "Avoid async void methods")]
        private async void CommandHandler(object sender, ExecutedRoutedEventArgs ea)
        {
            try
            {
                const string filter = "Image files (*.gif;*.jpg;*.png;*.webp;*.mp4)|*.gif;*.jpg;*.png;*.webp;*.mp4";

                using var ofd = new OpenFileDialog {
                    Filter = filter
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

                        using var reader  = new StreamReader(stream);
                        var       message = await reader.ReadToEndAsync().ConfigureAwait(false);
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
            catch (Exception ex)
            {
                TraceService.Message(ex.Message);
            }
        }

        private async ValueTask<MediaInfo> UploadMediaAsync(string path)
        {
            var contentType = ComposeControlViewModel.ContentType(path);
            var mediaId     = await UploadAsync(path, contentType).ConfigureAwait(false);
            return new MediaInfo { Path = path, MediaId = mediaId };
        }

        private async ValueTask<string> UploadAsync(string filename, string mediaType)
        {
            var bytes = await File.ReadAllBytesAsync(filename).ConfigureAwait(false);
            var media = await TwitterService.TwitterApi.UploadMediaInit(bytes.Length, mediaType).ConfigureAwait(false);
            if (media.MediaId is null) throw new Exception("error processing image init in UplaodAsync");
            await TwitterService.TwitterApi.UploadMediaAppend(media.MediaId, 0, bytes).ConfigureAwait(false);
            var finalize = await TwitterService.TwitterApi.UploadMediaFinalize(media.MediaId).ConfigureAwait(false);

            if (finalize.ProcessingInfo is not null)
            {
                await UntilProcessingFinishedAsync(media.MediaId).ConfigureAwait(false);
            }

            return media.MediaId;
        }

        private async ValueTask UntilProcessingFinishedAsync(string mediaId)
        {
            while (true)
            {
                var status = await TwitterService.TwitterApi.UploadMediaStatus(mediaId).ConfigureAwait(false);
                if (status.ProcessingInfo is null) throw new Exception("Image status processingInfo missing in UntilProcessingFinishedAsync");
                if (status.ProcessingInfo.State.IsEqualTo(ProcessingInfo.StateSucceeded)) break;
                if (status.ProcessingInfo.Error.Code != 0) throw new Exception(status.ProcessingInfo.Error.Message);
                if (status.ProcessingInfo.CheckAfterSecs == 0) throw new Exception("Image status corrupted in UntilProcessingFinishedAsync");
                var milliseconds = (int)TimeSpan.FromSeconds(status.ProcessingInfo.CheckAfterSecs).TotalMilliseconds;
                await Task.Delay(milliseconds).ConfigureAwait(false);
            }
        }
    }
}