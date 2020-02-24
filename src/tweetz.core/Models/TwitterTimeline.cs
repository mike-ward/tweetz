using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using tweetz.core.Infrastructure;
using twitter.core.Models;

namespace tweetz.core.Models
{
    public class TwitterTimeline : NotifyPropertyChanged
    {
        public TwitterTimeline(ISettings settings, ISystemState systemState, double intervalInMinutes)
        {
            Settings = settings;
            SystemState = systemState;
            FadeInDuration = TimeSpan.Zero;
            IntervalInMinutes = intervalInMinutes;
            Settings.PropertyChanged += OnSettingsChanged;
        }

        public ISettings Settings { get; }
        public ISystemState SystemState { get; }

        public double IntervalInMinutes { get; }
        public HashSet<string> AlreadyAdded { get; } = new HashSet<string>();
        public Duration FadeInDuration { get => fadeInDuration; set => SetProperty(ref fadeInDuration, value); }
        public string? ExceptionMessage { get => exceptionMessage; set => SetProperty(ref exceptionMessage, value); }
        public ObservableCollection<TwitterStatus> StatusCollection { get; set; } = new ObservableCollection<TwitterStatus>();

        public bool IsScrolled
        {
            get => isScrolled;
            set
            {
                SetProperty(ref isScrolled, value);
                ExceptionMessage = isScrolled && Settings.PauseWhenScrolled
                    ? (string)Application.Current.FindResource("paused-due-to-scroll-pos")
                    : null;
            }
        }

        private bool inUpdate;
        private bool isScrolled;
        private DispatcherTimer? timer;
        private Duration fadeInDuration;
        private string? exceptionMessage;
        private readonly List<Func<TwitterTimeline, Task>> updateTasks = new List<Func<TwitterTimeline, Task>>();

        public void AddUpdateTask(Func<TwitterTimeline, Task> task)
        {
            updateTasks.Add(task);
        }

        private async void OnSettingsChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Settings.IsAuthenticated))
            {
                if (Settings.IsAuthenticated)
                {
                    await Start();
                }
                else
                {
                    Stop();
                }
            }
        }

        private async Task Start()
        {
            if (timer != null) return;
            timer = new DispatcherTimer { Interval = TimeSpan.FromMinutes(IntervalInMinutes) };
            timer.Tick += async (s, args) => await Update();
            await Update();
            timer.Start();
        }

        private void Stop()
        {
            timer?.Stop();
            timer = null;
            AlreadyAdded.Clear();
            StatusCollection.Clear();
        }

        public async Task Update()
        {
            try
            {
                if (inUpdate) return;
                inUpdate = true;

                if (SystemState.IsSleeping) return;
                if (IsScrolled && Settings.PauseWhenScrolled) return;

                Trace.TraceInformation("Updating timeline");

                foreach (var updateTask in updateTasks)
                {
                    await updateTask(this);
                }

                ExceptionMessage = null;
            }
            catch (Exception ex)
            {
                ExceptionMessage = ex.Message;
            }
            finally
            {
                inUpdate = false;
            }
        }
    }
}