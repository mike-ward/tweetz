﻿using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using tweetz.core.Interfaces;
using tweetz.core.ViewModels;
using tweetz.core.Views.MediaViewerBlock;
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
            // placement rectangle needs to be set every time to handle multiple
            // monitors (e.g. program moved to different monitor while running)
            mediaViewerBlock.Popup.PlacementRectangle = Screen.ScreenRectFromWindow(Application.Current.MainWindow!);

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
            ArgumentNullException.ThrowIfNull(media);

            if (media.VideoInfo?.Variants?[0] is null)
            {
                return new Uri(media.MediaUrl);
            }

            var url = media.VideoInfo.Variants
               .Where(variant => IsMp4(variant.Url))
               .Select(variant => variant.Url)
               .FirstOrDefault();

            return url is not null
                ? new Uri(url)
                : new Uri(media.MediaUrl ?? throw new InvalidOperationException("expected MediaUrl"));
        }

        public static bool IsMp4(string? url)
        {
            return !string.IsNullOrWhiteSpace(url)
                && url.Contains(".mp4", StringComparison.OrdinalIgnoreCase);
        }

        public static void CopyUIElementToClipboard(FrameworkElement element, Uri? uri)
        {
            try
            {
                var width      = element.ActualWidth;
                var height     = element.ActualHeight;
                var dataObject = new DataObject();

                if (uri is null)
                {
                    var dv = new DrawingVisual();
                    var vb = new VisualBrush(element);

                    using var dc = dv.RenderOpen();
                    dc.DrawRectangle(vb, pen: null, new Rect(new Point(), new Size(width, height)));
                    dc.Close();

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
                TraceService.Message(ex.Message);
            }
        }

        public static MediaState GetMediaState(MediaElement media)
        {
            // Yeah, had to resort to refection
            var hlp          = typeof(MediaElement).GetField("_helper", BindingFlags.NonPublic | BindingFlags.Instance)!;
            var helperObject = hlp.GetValue(media)!;
            var stateField   = helperObject.GetType().GetField("_currentState", BindingFlags.NonPublic | BindingFlags.Instance);
            return (MediaState)stateField!.GetValue(helperObject)!;
        }
    }
}