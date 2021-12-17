using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using tweetz.core.Extensions;
using tweetz.core.Interfaces;
using tweetz.core.Services;
using twitter.core.Models;

namespace tweetz.core.Models
{
    public class TwitterTimeline : NotifyPropertyChanged
    {
        public  ISettings    Settings          { get; }
        private ISystemState SystemState       { get; }
        private double       IntervalInMinutes { get; }

        public ISet<string> AlreadyAdded { get; } = new HashSet<string>(StringComparer.Ordinal);

        protected string? ExceptionMessage
        {
            get => exceptionMessage;
            set => SetProperty(ref exceptionMessage, value);
        }

        public ObservableCollectionEx<TwitterStatus> StatusCollection        { get; } = new();
        public ISet<TwitterStatus>                   PendingStatusCollection { get; } = new HashSet<TwitterStatus>();

        public bool PendingStatusesAvailable
        {
            get => pendingStatusesAvailable;
            set => SetProperty(ref pendingStatusesAvailable, value);
        }

        private string? ToolTipText
        {
            // ReSharper disable once UnusedMember.Local
            get => toolTipText;
            set => SetProperty(ref toolTipText, value);
        }

        protected        string                                 timelineName = "unknown";
        private          bool                                   inUpdate;
        private          bool                                   isScrolled;
        private          string?                                exceptionMessage;
        private          bool                                   pendingStatusesAvailable;
        private          string?                                toolTipText;
        private readonly DispatcherTimer                        updateTimer;
        private readonly List<Func<TwitterTimeline, ValueTask>> updateTasks = new();

        protected TwitterTimeline(ISettings settings, ISystemState systemState, double intervalInMinutes)
        {
            Settings          = settings;
            SystemState       = systemState;
            IntervalInMinutes = intervalInMinutes;

            updateTimer = new DispatcherTimer { Interval = TimeSpan.FromMinutes(IntervalInMinutes) };
            updateTimer.Tick += delegate
            {
                var unused = UpdateAsync();
            };
            PropertyChanged          += UpdateTooltip;
            Settings.PropertyChanged += OnAuthenticationChanged;
        }

        [SuppressMessage("Usage", "VSTHRD100", MessageId = "Avoid async void methods")]
        private async void OnAuthenticationChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.IsEqualTo(nameof(Settings.IsAuthenticated)))
            {
                if (Settings.IsAuthenticated)
                {
                    await Start().ConfigureAwait(false);
                }
                else
                {
                    Stop();
                }
            }
        }

        private async Task Start()
        {
            if (!updateTimer.IsEnabled)
            {
                updateTimer.Start();
                await UpdateAsync().ConfigureAwait(false);
            }
        }

        private void Stop()
        {
            updateTimer.Stop();
            AlreadyAdded.Clear();
            StatusCollection.Clear();
        }

        protected void AddUpdateTask(Func<TwitterTimeline, ValueTask> task)
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

                inUpdate         = true;
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
            StatusCollection.InsertRange(PendingStatusCollection.OrderBy(o => o.CreatedDate));
            PendingStatusCollection.Clear();
            PendingStatusesAvailable = false;
        }

        [SuppressMessage("Usage", "VSTHRD001", MessageId = "Avoid legacy thread switching APIs")]
        private void UpdateTooltip(object? sender, PropertyChangedEventArgs? e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (PendingStatusesAvailable) { ToolTipText          = App.GetString("new-tweets-arrived-tooltip"); }
                else if (ExceptionMessage is not null) { ToolTipText = ExceptionMessage; }
                else { ToolTipText                                   = null; }
            });
        }
    }
}