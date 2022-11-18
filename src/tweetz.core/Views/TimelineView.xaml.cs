using System;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using tweetz.core.Interfaces;
using tweetz.core.Models;
using tweetz.core.Services;
using tweetz.core.ViewModels;

namespace tweetz.core.Views
{
    public partial class TimelineView : UserControl
    {
        private ISettings              Settings              { get; }
        private ISystemTrayIconService SystemTrayIconService { get; }

        private static readonly ThicknessAnimation SlideDownAnimation =
            new(fromValue: new Thickness(0, -80, 0, 0),
                toValue: new Thickness(0),
                duration: TimeSpan.FromMilliseconds(80));

        public TimelineView()
        {
            InitializeComponent();
            Settings              =  App.ServiceProvider.GetService<ISettings>();
            SystemTrayIconService =  App.ServiceProvider.GetService<ISystemTrayIconService>();
            Loaded                += TimelineControl_Loaded;
        }

        private void TimelineControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is HomeTimelineControlViewModel vm)
            {
                vm.StatusCollection.CollectionChanged += delegate
                {
                    AnimateScroll(vm);
                    UpdateAppIcon();
                    PlayNotifySound();
                };
            }
        }

        private void AnimateScroll(TwitterTimeline vm)
        {
            if (!vm.IsScrolled)
            {
                ItemsControl.BeginAnimation(MarginProperty, SlideDownAnimation);
            }
        }

        private void UpdateAppIcon()
        {
            var isMinimized = Application.Current.MainWindow?.WindowState == WindowState.Minimized;
            SystemTrayIconService.UpdateIcon(isMinimized);
            ((MainWindow)Application.Current.MainWindow!)?.UpdateAppIcon(isMinimized);
        }

        private void PlayNotifySound()
        {
            try
            {
                if (Settings.PlayNotifySound)
                {
                    if (!string.IsNullOrWhiteSpace(Settings.NotifySoundSource))
                    {
                        var player = new SoundPlayer(Settings.NotifySoundSource);
                        player.Play();
                    }
                    else
                    {
                        SystemSounds.Asterisk.Play();
                    }
                }
            }
            catch (Exception ex)
            {
                TraceService.Message(ex.Message);
            }
        }

        public void ScrollToHome()
        {
            // is there an easier way to find the ScrollViewer?
            if (VisualTreeHelper.GetChildrenCount(ItemsControl) > 0)
            {
                var scrollViewer = VisualTreeHelper.GetChild(ItemsControl, 0) as ScrollViewer;
                scrollViewer?.ScrollToHome();
            }
        }

        private void OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (DataContext is TwitterTimeline model)
            {
                model.IsScrolled = e.VerticalOffset > 0;

                if (!model.IsScrolled)
                {
                    model.AddPendingToStatusCollection();
                    ScrollToHome();
                }
            }
        }
    }
}