using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
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

            updateTimer = new DispatcherTimer { Interval = TimeSpan.FromMinutes(IntervalInMinutes) };
            updateTimer.Tick += (_, __) => Update();

            Settings.PropertyChanged += OnAuthenticationChanged;
            BindingOperations.EnableCollectionSynchronization(StatusCollection, SyncObject);
        }

        private void OnAuthenticationChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Settings.IsAuthenticated))
            {
                if (Settings.IsAuthenticated)
                {
                    Start();
                }
                else
                {
                    Stop();
                }
            }
        }

        private void Start()
        {
            if (!updateTimer.IsEnabled)
            {
                updateTimer.Start();
                Update();
            }
        }

        private void Stop()
        {
            updateTimer?.Stop();
            AlreadyAdded.Clear();
            StatusCollection.Clear();
        }

        public void AddUpdateTask(Func<TwitterTimeline, ValueTask> task)
        {
            updateTasks.Add(task);
        }

        private void Update()
        {
            // Fire and forget pattern
            UpdateAsync().ConfigureAwait(false);
        }

        public async ValueTask UpdateAsync()
        {
            if (inUpdate) { Trace.TraceInformation($"{timelineName}: inUpdate"); return; }
            if (SystemState.IsSleeping) { Trace.TraceInformation($"{timelineName}: isSleeping"); return; }
            if (IsScrolled && Settings.PauseWhenScrolled) { Trace.TraceInformation($"{timelineName}: isPaused"); return; }

            try
            {
                inUpdate = true;
                Trace.TraceInformation($"{timelineName}: Updating");

                foreach (var updateTask in updateTasks)
                {
                    await updateTask(this).ConfigureAwait(false);
                }

                ExceptionMessage = null;
            }
            catch (Exception ex)
            {
                Trace.TraceError($"{timelineName}: ${ex.Message}");
                ExceptionMessage = ex.Message;
            }
            finally
            {
                inUpdate = false;
            }
        }

        public ISettings Settings { get; }
        public ISystemState SystemState { get; }

        public double IntervalInMinutes { get; }
        public HashSet<string> AlreadyAdded { get; } = new HashSet<string>();
        public Duration FadeInDuration { get => fadeInDuration; set => SetProperty(ref fadeInDuration, value); }
        public string? ExceptionMessage { get => exceptionMessage; set => SetProperty(ref exceptionMessage, value); }
        public ObservableCollection<TwitterStatus> StatusCollection { get; } = new ObservableCollection<TwitterStatus>();

        public bool IsScrolled
        {
            get => isScrolled;
            set
            {
                SetProperty(ref isScrolled, value);

                // ExceptionMessage is used by the TabItemIndicatorAdorner to show messages to the user
                ExceptionMessage = isScrolled && Settings.PauseWhenScrolled
                    ? (string)Application.Current.FindResource("paused-due-to-scroll-pos")
                    : null;
            }
        }

        protected string timelineName = "unknown";
        private bool inUpdate;
        private bool isScrolled;
        private Duration fadeInDuration;
        private string? exceptionMessage;
        private readonly DispatcherTimer updateTimer;
        private readonly List<Func<TwitterTimeline, ValueTask>> updateTasks = new List<Func<TwitterTimeline, ValueTask>>();
        private object SyncObject => new object();
    }
}