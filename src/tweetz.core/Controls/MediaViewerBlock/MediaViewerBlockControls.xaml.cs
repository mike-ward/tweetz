using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
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
            timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.25) };
            timer.Tick += Timer_Tick;

            MediaElement.MediaOpened += (_, __) =>
            {
                timer.Start();
                SetPlayPauseButtonSymbol(PlayPauseButton, PauseButtonSymbol);

                var visibility = MediaElement.NaturalDuration.HasTimeSpan ? Visibility.Visible : Visibility.Collapsed;
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

            if (MediaElement.Source == null)
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

            var state = GetMediaState(MediaElement);

            if (state == MediaState.Play)
            {
                MediaElement.Pause();
                SetPlayPauseButtonSymbol((Button)sender, PlayButtonSymbol);
            }
            else
            {
                MediaElement.Play();
                SetPlayPauseButtonSymbol((Button)sender, PauseButtonSymbol);
            }
        }

        private void SetPlayPauseButtonSymbol(Button button, string symbol)
        {
            button.Content = Application.Current.FindResource(symbol);
        }

        private void Rewind_Click(object sender, RoutedEventArgs e)
        {
            MediaElement.Position = TimeSpan.Zero;
            e.Handled = true;
        }

        private static MediaState GetMediaState(MediaElement media)
        {
            // Yeah, had to resort to refection
            var hlp = typeof(MediaElement).GetField("_helper", BindingFlags.NonPublic | BindingFlags.Instance)!;
            var helperObject = hlp.GetValue(media)!;
            var stateField = helperObject.GetType().GetField("_currentState", BindingFlags.NonPublic | BindingFlags.Instance);
            return (MediaState)stateField!.GetValue(helperObject)!;
        }
    }
}