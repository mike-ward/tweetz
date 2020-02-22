using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using tweetz.core.Infrastructure;
using twitter.core.Models;

namespace tweetz.core.Services
{
    public class ImageViewerService : IImageViewerService
    {
        public Popup CreatePopup(Window window, Uri uri)
        {
            var popupChild = CreatePopupChild(uri);

            var popup = new Popup
            {
                Child = popupChild,
                Placement = PlacementMode.Center,
                PlacementRectangle = Screen.ScreenRectFromWindow(window),
                PopupAnimation = PopupAnimation.Fade,
                SnapsToDevicePixels = true,
                UseLayoutRounding = true,
                AllowsTransparency = true
            };

            popup.KeyDown += (s, args) => popup.IsOpen = false;
            popup.MouseDown += (s, args) => popup.IsOpen = false;
            popup.IsOpen = true;
            return popup;
        }

        private static UIElement CreatePopupChild(Uri uri)
        {
            var popupChild = new Border
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1),
                Background = Brushes.Transparent,
                Child = CreateImageViewer(uri)
            };
            return popupChild;
        }

        private static Grid CreateImageViewer(Uri uri)
        {
            var loadingIndicator = new ProgressBar
            {
                Width = 100,
                Height = 10,
                IsIndeterminate = true,
            };

            var errorMessage = new TextBlock
            {
                FontSize = 16,
                Background = Brushes.White,
                Foreground = Brushes.Black,
                Padding = new Thickness(30),
                TextWrapping = TextWrapping.Wrap,
                Visibility = Visibility.Collapsed
            };

            var mediaElement = new MediaElement();
            var mediaControls = MediaControls(mediaElement);
            var clipboardCopy = ClipboardCopyControl();

            AddRepeatForever(uri, mediaElement);
            AddImageGrowAnimation(mediaElement);
            AddEventHandlers(loadingIndicator, mediaElement, mediaControls, clipboardCopy, errorMessage, uri);

            var grid = new Grid();
            grid.Children.Add(loadingIndicator);
            grid.Children.Add(errorMessage);
            grid.Children.Add(mediaElement);
            grid.Children.Add(mediaControls);
            grid.Children.Add(clipboardCopy);

            return grid;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1854:Unused assignments should be removed", Justification = "false positive")]
        private static FrameworkElement MediaControls(MediaElement mediaElement)
        {
            var timerBackgroundBrush = new SolidColorBrush(Colors.White) { Opacity = 0.3 };
            timerBackgroundBrush.Freeze();

            var timeDisplay = new TextBlock
            {
                Width = 100,
                Foreground = Brushes.Black,
                Background = timerBackgroundBrush,
                FontWeight = FontWeights.DemiBold,
                Visibility = Visibility.Collapsed,
                Margin = new Thickness(0, 0, 3, 2),
                TextAlignment = TextAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Right,
                FontFamily = new FontFamily("Lucida Sans Typewriter,Courier New"),
            };

            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };

            void tick(object? s, EventArgs args)
            {
                if (!mediaElement.NaturalDuration.HasTimeSpan) return;

                var time =
                    $"{mediaElement.Position.ToString(@"mm\:ss", CultureInfo.InvariantCulture)}/" +
                    $"{mediaElement.NaturalDuration.TimeSpan.ToString(@"mm\:ss", CultureInfo.InvariantCulture)}";

                timeDisplay.Text = time;
            }

            void StartTimer(object sender, RoutedEventArgs args)
            {
                timer.Tick += tick;
                timer.Start();
                timeDisplay.Loaded -= StartTimer;
            }

            timeDisplay.Loaded += StartTimer;

            void StopTimer(object sender, RoutedEventArgs args)
            {
                timer.Stop();
                timer.Tick -= tick;
                timeDisplay.Unloaded -= StopTimer;
            }

            timeDisplay.Unloaded += StopTimer;
            return timeDisplay;
        }

        private static readonly RoutedCommand CopyToClipboardCommand = new RoutedUICommand();

        private static TextBlock ClipboardCopyControl()
        {
            var backgroundBrush = new SolidColorBrush(Colors.White) { Opacity = 0.3 };
            backgroundBrush.Freeze();

            var block = new TextBlock
            {
                Cursor = Cursors.Hand,
                Foreground = Brushes.Black,
                Background = backgroundBrush,
                Visibility = Visibility.Collapsed,
                Margin = new Thickness(3, 0, 0, 2),
                TextAlignment = TextAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Left,
                Text = (string)Application.Current.FindResource("copy"),
                ToolTip = (string)Application.Current.FindResource("copy-to-clipboard"),
                FontFamily = new FontFamily("Lucida Sans Typewriter,Courier New")
            };

            const int delayInMilliseconds = 1250;
            ToolTipService.SetInitialShowDelay(block, delayInMilliseconds);
            block.InputBindings.Add(new InputBinding(CopyToClipboardCommand, new MouseGesture(MouseAction.LeftClick)));
            return block;
        }

        private static void AddRepeatForever(Uri uri, MediaElement mediaElement)
        {
            var mediaTimeline = new MediaTimeline
            {
                Source = uri,
                RepeatBehavior = RepeatBehavior.Forever
            };

            var storyboard = new Storyboard();
            storyboard.Children.Add(mediaTimeline);
            var beginStoryboard = new BeginStoryboard { Storyboard = storyboard };

            var eventTrigger = new EventTrigger { RoutedEvent = FrameworkElement.LoadedEvent };
            eventTrigger.Actions.Add(beginStoryboard);
            mediaElement.Triggers.Add(eventTrigger);
        }

        private static void AddImageGrowAnimation(MediaElement mediaElement)
        {
            var scaleTransform = new ScaleTransform { ScaleX = 1.25, ScaleY = 1.25 };
            mediaElement.RenderTransform = scaleTransform;
            mediaElement.RenderTransformOrigin = (new Point(0.5, 0.5));

            static DoubleAnimation AnimationFactory() => new DoubleAnimation
            {
                From = 0,
                To = 1.0,
                Duration = new Duration(TimeSpan.FromMilliseconds(100)),
            };

            var animationX = AnimationFactory();
            Storyboard.SetTarget(animationX, mediaElement);
            Storyboard.SetTargetProperty(animationX, new PropertyPath("RenderTransform.ScaleX", null));

            var animationY = AnimationFactory();
            Storyboard.SetTarget(animationY, mediaElement);
            Storyboard.SetTargetProperty(animationY, new PropertyPath("RenderTransform.ScaleY", null));

            var animationFadeIn = AnimationFactory();
            animationFadeIn.Duration = new Duration(TimeSpan.FromMilliseconds(400));
            Storyboard.SetTarget(animationFadeIn, mediaElement);
            Storyboard.SetTargetProperty(animationFadeIn, new PropertyPath("Opacity", null));

            var storyboard = new Storyboard();
            storyboard.Children.Add(animationX);
            storyboard.Children.Add(animationY);
            storyboard.Children.Add(animationFadeIn);

            var beginStoryboard = new BeginStoryboard { Storyboard = storyboard };
            var eventTrigger = new EventTrigger { RoutedEvent = MediaElement.MediaOpenedEvent };

            eventTrigger.Actions.Add(beginStoryboard);
            mediaElement.Triggers.Add(eventTrigger);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1172:Unused method parameters should be removed", Justification = "false positive")]
        private static void AddEventHandlers(
            FrameworkElement loadingIndicator,
            MediaElement mediaElement,
            FrameworkElement mediaControls,
            FrameworkElement clipboardCopy,
            TextBlock errorMessage,
            Uri uri)
        {
            void OpenedHandler(object sender, RoutedEventArgs args)
            {
                loadingIndicator.Visibility = Visibility.Collapsed;
                if (mediaElement.NaturalDuration.HasTimeSpan) mediaControls.Visibility = Visibility.Visible;
                clipboardCopy.Visibility = Visibility.Visible;

                clipboardCopy.CommandBindings.Add(new CommandBinding(CopyToClipboardCommand, (s, a) =>
                {
                    a.Handled = true;
                    CopyUIElementToClipboard(mediaElement, uri);
                }));

                mediaElement.MediaOpened -= OpenedHandler;
            }

            mediaElement.MediaOpened += OpenedHandler;

            void FailedHander(object? sender, ExceptionRoutedEventArgs args)
            {
                loadingIndicator.Visibility = Visibility.Collapsed;
                mediaElement.Visibility = Visibility.Collapsed;
                mediaControls.Visibility = Visibility.Collapsed;
                clipboardCopy.Visibility = Visibility.Collapsed;
                errorMessage.Text = args.ErrorException.Message;
                errorMessage.Visibility = Visibility.Visible;
                mediaElement.MediaFailed -= FailedHander;
            }
            mediaElement.MediaFailed += FailedHander;
        }

        private static void CopyUIElementToClipboard(FrameworkElement element, Uri uri)
        {
            try
            {
                var width = element.ActualWidth;
                var height = element.ActualHeight;
                var bmpCopied = new RenderTargetBitmap((int)Math.Round(width), (int)Math.Round(height), 96, 96, PixelFormats.Default);

                var dv = new DrawingVisual();
                using (var dc = dv.RenderOpen())
                {
                    var vb = new VisualBrush(element);
                    dc.DrawRectangle(vb, null, new Rect(new Point(), new Size(width, height)));
                }
                bmpCopied.Render(dv);

                var dataObject = new DataObject();
                dataObject.SetData(DataFormats.Dib, bmpCopied);
                dataObject.SetData(DataFormats.Text, uri.ToString());
                Clipboard.SetDataObject(dataObject);
            }
            catch
            {
                // ignored
            }
        }

        public Uri MediaSource(Media media)
        {
            if (media.VideoInfo?.Variants?[0] == null)
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