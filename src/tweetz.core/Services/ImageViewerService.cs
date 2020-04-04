using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using tweetz.core.Controls.MediaViewerBlock;
using tweetz.core.Infrastructure;
using tweetz.core.ViewModels;
using twitter.core.Models;

namespace tweetz.core.Services
{
    public class ImageViewerService : IImageViewerService
    {
        private readonly MediaViewerBlock mediaViewerBlock;

        public ImageViewerService(MediaViewerBlockViewModel mediaViewerBlockViewModel)
        {
            // In order to place the media viewer in the middle of the screen it has to be created
            // without a parent window. Can't do that in Xaml. Popup placement rules are weird.
            mediaViewerBlock = new MediaViewerBlock { DataContext = mediaViewerBlockViewModel };
        }

        public void Open(Uri uri)
        {
            var mediaViewerBlockViewModel = (MediaViewerBlockViewModel)mediaViewerBlock.DataContext;
            mediaViewerBlockViewModel.Uri = uri;
        }

        public void Close()
        {
            var mediaViewerBlockViewModel = (MediaViewerBlockViewModel)mediaViewerBlock.DataContext;
            mediaViewerBlockViewModel.Uri = null;
        }

        public static Uri MediaSource(Media media)
        {
            if (media is null) throw new ArgumentNullException(nameof(media));

            if (media.VideoInfo?.Variants?[0] is null)
            {
                return new Uri(media.MediaUrl ?? string.Empty);
            }

            var url = media.VideoInfo.Variants
                .Where(variant => !string.IsNullOrWhiteSpace(variant.Url) && IsMp4(variant.Url))
                .Select(variant => variant.Url)
                .FirstOrDefault();

            return url != null
                ? new Uri(url)
                : new Uri(media.MediaUrl ?? throw new InvalidOperationException("expected MediaUrl"));
        }

        public static bool IsMp4(string url)
        {
            var findExtension = new Regex(@".+(\.\w{3})\?*.*");
            var result = findExtension.Match(url);

            return result.Success &&
                   result.Groups.Count > 1 &&
                   string.Equals(result.Groups[1].Value, ".mp4", StringComparison.InvariantCultureIgnoreCase);
        }

        public static void CopyUIElementToClipboard(FrameworkElement element, Uri? uri)
        {
            try
            {
                var width = element.ActualWidth;
                var height = element.ActualHeight;
                var dataObject = new DataObject();

                if (uri is null)
                {
                    var dv = new DrawingVisual();
                    using (var dc = dv.RenderOpen())
                    {
                        var vb = new VisualBrush(element);
                        dc.DrawRectangle(vb, null, new Rect(new Point(), new Size(width, height)));
                    }
                    var bmpCopied = new RenderTargetBitmap((int)Math.Round(width), (int)Math.Round(height), 96, 96, PixelFormats.Default);
                    bmpCopied.Render(dv);

                    dataObject.SetData(DataFormats.Dib, bmpCopied);
                }
                else
                {
                    dataObject.SetData(DataFormats.Text, uri.ToString());
                }

                Clipboard.SetDataObject(dataObject);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
            }
        }

        public static MediaState GetMediaState(MediaElement media)
        {
            // Yeah, had to resort to refection
            var hlp = typeof(MediaElement).GetField("_helper", BindingFlags.NonPublic | BindingFlags.Instance)!;
            var helperObject = hlp.GetValue(media)!;
            var stateField = helperObject.GetType().GetField("_currentState", BindingFlags.NonPublic | BindingFlags.Instance);
            return (MediaState)stateField!.GetValue(helperObject)!;
        }
    }
}