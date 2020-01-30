using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
using tweetz.core.Infrastructure;
using tweetz.core.Models;
using twitter.core.Models;

namespace tweetz.core.ViewModels
{
    public abstract class BaseTimelineControlViewModel : TwitterStatusControlViewModel
    {
        private bool inUpdate;
        private DispatcherTimer? timer;
        private ISystemState SystemState { get; }

        protected BaseTimelineControlViewModel(ISettings settings, ISystemState systemState)
            : base(settings)
        {
            SystemState = systemState;
            Settings.PropertyChanged += OnSettingsChanged;
        }

        protected abstract Task<IEnumerable<TwitterStatus>> GetTimeline();

        protected abstract double IntervalInMinutes { get; }

        private async void OnSettingsChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Settings.IsAuthenticated))
            {
                if (Settings.IsAuthenticated) await Start().ConfigureAwait(true);
                else Stop();
            }
        }

        private async Task Start()
        {
            if (timer != null) return;
            timer = new DispatcherTimer { Interval = TimeSpan.FromMinutes(IntervalInMinutes) };
            timer.Tick += async (s, args) => await Update().ConfigureAwait(true);
            timer.Start();
            await Update().ConfigureAwait(true);
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

                var statuses = await GetTimeline().ConfigureAwait(true);
                UpdateTimeline(statuses);
                DonateNag();
                TruncateStatusCollection();
                UpdateTimeStamps();
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

        // Once the upper limit of StatusCollections is reached, the oldest
        // statuses are removed. AlreadyAdded<> ensures that the status won't
        // appear again. Why do we need this? The home timeline contains
        // mentions (see HomeTimelineControlViewModel.cs). Without this check,
        // mentions can reappear after they leave StatusCollection. So why not
        // use the sinceId parameter then when querying the timeline? We need
        // the old statues to update counts for retweets, favorites, etc.
        private HashSet<string> AlreadyAdded { get; } = new HashSet<string>();

        // use this to disconnet the binding from the status collection while
        // updating. This reduces jank in the timeline
        protected static readonly ObservableCollection<TwitterStatus> EmptyStatusCollection = new ObservableCollection<TwitterStatus>();

        public void UpdateTimeline(IEnumerable<TwitterStatus> statuses)
        {
            // ObservableCollection only supports linear searching.
            // Build a dictionary for faster lookups.
            var statusDictionary = StatusCollection
                .Where(status => status.Id != DonateNagStatus.DonateNagStatusId)
                .ToDictionary(status => status.Id, status => status);

            // disconnet the binding from the status collection while updating.
            // This reduces jank in the timeline
            var current = StatusCollection;
            StatusCollection = EmptyStatusCollection;

            try
            {
                foreach (var status in statuses.Reverse())
                {
                    if (statusDictionary.TryGetValue(status.Id, out var statusToUpdate))
                    {
                        statusToUpdate.OriginatingStatus.UpdateFromStatus(status.OriginatingStatus);
                    }
                    else if (!AlreadyAdded.Contains(status.Id))
                    {
                        AlreadyAdded.Add(status.Id);
                        status.AboutMe(Settings.ScreenName);
                        current.Insert(0, status);
                    }
                }
            }
            finally
            {
                StatusCollection = current;
            }
        }

        private void TruncateStatusCollection()
        {
            var maxNumberOfStatuses = 500;

            while (StatusCollection.Count > maxNumberOfStatuses)
            {
                StatusCollection.RemoveAt(StatusCollection.Count - 1);
            }
        }

        private void UpdateTimeStamps()
        {
            foreach (var status in StatusCollection)
            {
                status.InvokeUpdateTimeStamp();
            }
        }

        private const int donateNagCounterInterval = 120;
        private int donateNagCounter = donateNagCounterInterval - 10;

        private void DonateNag()
        {
            if (Settings.Donated) return;

            if (donateNagCounter >= donateNagCounterInterval)
            {
                donateNagCounter = 0;
                StatusCollection.Insert(0, new DonateNagStatus());
            }
            else
            {
                donateNagCounter += 1;
            }
        }
    }
}