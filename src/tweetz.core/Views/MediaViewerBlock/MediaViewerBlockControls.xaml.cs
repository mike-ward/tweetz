using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using tweetz.core.Services;
using tweetz.core.ViewModels;

namespace tweetz.core.Views.MediaViewerBlock
{
    public partial class MediaViewerBlockControls : UserControl
    {
        public MediaViewerBlockControls()
        {
            InitializeComponent();
            Loaded += MediaViewerBlockControls_Loaded;
        }

        public static readonly DependencyProperty MediaElementProperty =
            DependencyProperty.Register(nameof(MediaElement), typeof(MediaElement), typeof(MediaViewerBlockControls));

        public MediaElement MediaElement
        {
            get => (MediaElement)GetValue(MediaElementProperty);
            set => SetValue(MediaElementProperty, value);
        }

        public static readonly DependencyProperty ImageElementProperty = DependencyProperty.Register(
            nameof(ImageElement), typeof(Image), typeof(MediaViewerBlockControls), new PropertyMetadata(default(Image)));

        public Image ImageElement
        {
            get => (Image)GetValue(ImageElementProperty);
            set => SetValue(ImageElementProperty, value);
        }

        private DispatcherTimer? timer;

        private const string PlayButtonSymbol  = "PlaySymbol";
        private const string PauseButtonSymbol = "PauseSymbol";

        private void MediaViewerBlockControls_Loaded(object sender, RoutedEventArgs e)
        {
            timer                    =  new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.5) };
            timer.Tick               += Timer_Tick;
            MediaElement.MediaOpened += OnMediaOpened;
        }

        private void OnMediaOpened(object _, RoutedEventArgs __)
        {
            timer?.Start();
            SetPlayPauseButtonSymbol(PlayPauseButton, PauseButtonSymbol);

            var visibility = MediaElement.NaturalDuration.HasTimeSpan
                ? Visibility.Visible
                : Visibility.Collapsed;

            SetControlVisibilities(visibility);
        }

        public void SetControlVisibilities(Visibility visibility)
        {
            RewindButton.Visibility       = visibility;
            TenSecRewindButton.Visibility = visibility;
            PlayPauseButton.Visibility    = visibility;
            ProgressIndicator.Visibility  = visibility;
        }

        private void PlayPause_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            if (!MediaElement.NaturalDuration.HasTimeSpan) return;
            TogglePlayPauseState((Button)sender);
        }

        private void CopyUriToClipboard_Click(object sender, RoutedEventArgs e)
        {
            if (MediaElement.Source is not null) ImageViewerService.CopyUIElementToClipboard(MediaElement, MediaElement.Source);
            else ImageViewerService.CopyUIElementToClipboard(ImageElement, ((MediaViewerBlockViewModel)ImageElement.DataContext).Uri);
        }

        private void CopyImageToClipboard_Click(object sender, RoutedEventArgs e)
        {
            ImageViewerService.CopyUIElementToClipboard(MediaElement.Source is not null
                    ? MediaElement
                    : ImageElement,
                uri: null);
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (MediaElement.NaturalDuration.HasTimeSpan)
            {
                ProgressIndicator.Maximum = MediaElement.NaturalDuration.TimeSpan.TotalSeconds;
                ProgressIndicator.Value   = MediaElement.Position.TotalSeconds;
            }

            if (MediaElement.Source is null)
            {
                timer?.Stop();
                ProgressIndicator.Value = 0;
            }
        }

        private void TogglePlayPauseState(Button button)
        {
            var state = ImageViewerService.GetMediaState(MediaElement);

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

        private static void SetPlayPauseButtonSymbol(Button button, string symbol)
        {
            button.Content = Application.Current.FindResource(symbol);
        }

        private void Rewind_Click(object sender, RoutedEventArgs e)
        {
            e.Handled             = true;
            MediaElement.Position = TimeSpan.Zero;
        }

        private void Rewind_Ten_Seconds_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;

            MediaElement.Position = MediaElement.Position.TotalSeconds > 10
                ? MediaElement.Position - TimeSpan.FromSeconds(10)
                : TimeSpan.Zero;

            ProgressIndicator.Value = MediaElement.Position.TotalSeconds;
        }
    }
}