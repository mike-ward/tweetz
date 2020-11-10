using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using tweetz.core.Infrastructure;
using tweetz.core.Infrastructure.Extensions;
using tweetz.core.Services;
using twitter.core.Models;

namespace tweetz.core.Models
{
    public class TwitterTimeline : NotifyPropertyChanged
    {
        public ISettings Settings { get; }
        public ISystemState SystemState { get; }

        public double IntervalInMinutes { get; }
        public ISet<string> AlreadyAdded { get; } = new HashSet<string>(StringComparer.Ordinal);
        public string? ExceptionMessage { get => exceptionMessage; set => SetProperty(ref exceptionMessage, value); }
        public ObservableCollection<TwitterStatus> StatusCollection { get; } = new ObservableCollection<TwitterStatus>();
        public ISet<TwitterStatus> PendingStatusCollection { get; } = new HashSet<TwitterStatus>();
        public bool PendingStatusesAvailable { get => pendingStatusesAvailable; set => SetProperty(ref pendingStatusesAvailable, value); }
        public string? ToolTipText { get => toolTipText; set => SetProperty(ref toolTipText, value); }

        protected string timelineName = "unknown";
        private bool inUpdate;
        private bool isScrolled;
        private string? exceptionMessage;
        private bool pendingStatusesAvailable;
        private string? toolTipText;
        private readonly DispatcherTimer updateTimer;
        private readonly List<Func<TwitterTimeline, ValueTask>> updateTasks = new();

        public TwitterTimeline(ISettings settings, ISystemState systemState, double intervalInMinutes)
        {
            Settings = settings;
            SystemState = systemState;
            IntervalInMinutes = intervalInMinutes;

            updateTimer = new DispatcherTimer { Interval = TimeSpan.FromMinutes(IntervalInMinutes) };
            updateTimer.Tick += async (_, __) => await UpdateAsync().ConfigureAwait(false);

            PropertyChanged += UpdateTooltip;
            Settings.PropertyChanged += OnAuthenticationChanged;
        }

        private async void OnAuthenticationChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.IsEqualTo(nameof(Settings.IsAuthenticated)))
            {
                if (Settings.IsAuthenticated)
                {
                    if (!updateTimer.IsEnabled)
                    {
                        updateTimer.Start();
                        await UpdateAsync().ConfigureAwait(false);
                    }
                }
                else
                {
                    Stop();
                }
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

        public async ValueTask UpdateAsync()
        {
            try
            {
                if (inUpdate)
                {
                    TraceService.Message($"{timelineName} inUpdate");
                    return;
                }

                inUpdate = true;
                ExceptionMessage = null;

                if (SystemState.IsSleeping)
                {
                    TraceService.Message($"{timelineName}: isSleeping");
                    return;
                }

                TraceService.Message($"{timelineName}: Updating");

                foreach (var updateTask in updateTasks)
                {
                    await updateTask(this).ConfigureAwait(true);
                }
            }
            catch (Exception ex)
            {
                TraceService.Message($"{timelineName}: ${ex.Message}");
                ExceptionMessage = $"{ex.Message}";
            }
            finally
            {
                inUpdate = false;
            }
        }

        public bool IsScrolled
        {
            get => isScrolled;
            set => SetProperty(ref isScrolled, value);
        }

        public void AddPendingToStatusCollection()
        {
            foreach (var pendingStatus in PendingStatusCollection.OrderBy(o => o.OriginatingStatus.CreatedDate))
            {
                StatusCollection.Insert(0, pendingStatus);
            }

            PendingStatusCollection.Clear();
            PendingStatusesAvailable = false;
        }

        private void UpdateTooltip(object? sender, PropertyChangedEventArgs? e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (PendingStatusesAvailable) { ToolTipText = (string)Application.Current.FindResource("new-tweets-arrived-tooltip"); }
                else if (ExceptionMessage is not null) { ToolTipText = ExceptionMessage; }
                else { ToolTipText = null; }
            });
        }
    }
}