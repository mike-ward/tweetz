using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace tweetz.core.Controls.MediaViewerBlock
{
    public partial class MediaViewerBlockControls : UserControl
    {
        private DispatcherTimer timer;
        private const string PlayButtonSymbol = "PlaySymbol";
        private const string PauseButtonSymbol = "PauseSymbol";

        public MediaViewerBlockControls()
        {
            InitializeComponent();
            Loaded += MediaViewerBlockControls_Loaded;
        }

        private void MediaViewerBlockControls_Loaded(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.5) };
            timer.Tick += Timer_Tick;

            MediaElement.MediaOpened += (_, __) =>
            {
                timer.Start();
                SetPlayPauseButtonSymbol(PlayPauseButton, PauseButtonSymbol);

                var visibility = MediaElement.NaturalDuration.HasTimeSpan
                    ? Visibility.Visible
                    : Visibility.Collapsed;

                RewindButton.Visibility = visibility;
                PlayPauseButton.Visibility = visibility;
                ProgressIndicator.Visibility = visibility;
            };
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (MediaElement.NaturalDuration.HasTimeSpan)
            {
                ProgressIndicator.Maximum = MediaElement.NaturalDuration.TimeSpan.TotalSeconds;
                ProgressIndicator.Value = MediaElement.Position.TotalSeconds;
            }

            if (MediaElement.Source is null)
            {
                timer.Stop();
                ProgressIndicator.Value = 0;
            }
        }

        public MediaElement MediaElement
        {
            get { return (MediaElement)GetValue(MediaElementProperty); }
            set { SetValue(MediaElementProperty, value); }
        }

        public static readonly DependencyProperty MediaElementProperty =
            DependencyProperty.Register("MediaElement", typeof(MediaElement), typeof(MediaViewerBlockControls));

        private void PlayPause_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            if (!MediaElement.NaturalDuration.HasTimeSpan) return;
            TogglePlayPauseState((Button)sender);
        }

        private void TogglePlayPauseState(Button button)
        {
            var state = GetMediaState(MediaElement);

            if (state == MediaState.Play)
            {
                MediaElement.Pause();
                SetPlayPauseButtonSymbol(button, PlayButtonSymbol);
            }
            else
            {
                MediaElement.Play();
                SetPlayPauseButtonSymbol(button, PauseButtonSymbol);
            }
        }

        private void SetPlayPauseButtonSymbol(Button button, string symbol)
        {
            button.Content = Application.Current.FindResource(symbol);
        }

        private void Rewind_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            MediaElement.Position = TimeSpan.Zero;
        }

        private static MediaState GetMediaState(MediaElement media)
        {
            // Yeah, had to resort to refection
            var hlp = typeof(MediaElement).GetField("_helper", BindingFlags.NonPublic | BindingFlags.Instance)!;
            var helperObject = hlp.GetValue(media)!;
            var stateField = helperObject.GetType().GetField("_currentState", BindingFlags.NonPublic | BindingFlags.Instance);
            return (MediaState)stateField!.GetValue(helperObject)!;
        }

        private void CopyToClipboard_Click(object sender, RoutedEventArgs e)
        {
            CopyUIElementToClipboard(MediaElement, MediaElement.Source);
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
    }
}