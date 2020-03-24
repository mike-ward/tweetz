using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace tweetz.core.Controls.MediaViewerBlock
{
    public partial class MediaViewerBlockControls : UserControl
    {
        private DispatcherTimer timer;

        public MediaViewerBlockControls()
        {
            InitializeComponent();
            Loaded += MediaViewerBlockControls_Loaded;
        }

        private void MediaViewerBlockControls_Loaded(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.25) };
            timer.Tick += Timer_Tick;

            MediaElement.MediaOpened += (_, __) => timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (MediaElement.NaturalDuration.HasTimeSpan)
            {
                Slider.Maximum = MediaElement.NaturalDuration.TimeSpan.TotalSeconds;
                Slider.Value = MediaElement.Position.TotalSeconds;
            }

            if (MediaElement.Source == null)
            {
                timer.Stop();
                Slider.Value = 0;
            }
        }

        public MediaElement MediaElement
        {
            get { return (MediaElement)GetValue(MediaElementProperty); }
            set { SetValue(MediaElementProperty, value); }
        }

        public static readonly DependencyProperty MediaElementProperty =
            DependencyProperty.Register("MediaElement", typeof(MediaElement), typeof(MediaViewerBlockControls));
    }
}