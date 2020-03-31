using System;
using System.Linq;
using System.Text.RegularExpressions;
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

        public Uri MediaSource(Media media)
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

        private static bool IsMp4(string url)
        {
            var findExtension = new Regex(@".+(\.\w{3})\?*.*");
            var result = findExtension.Match(url);

            return result.Success &&
                   result.Groups.Count > 1 &&
                   string.Equals(result.Groups[1].Value, ".mp4", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}