using System;
using System.Collections.Generic;
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

        public ISystemState SystemState { get; }

        public BaseTimelineControlViewModel(ISettings settings, ISystemState systemState)
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
                if (Settings.IsAuthenticated) await Start();
                else Stop();
            }
        }

        private async Task Start()
        {
            if (timer != null) return;
            timer = new DispatcherTimer { Interval = TimeSpan.FromMinutes(IntervalInMinutes) };
            timer.Tick += async (s, args) => await Update();
            timer.Start();
            await Update();
        }

        private void Stop()
        {
            timer?.Stop();
            timer = null;
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

                var statuses = await GetTimeline();
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

        public void UpdateTimeline(IEnumerable<TwitterStatus> statuses)
        {
            // ObservableCollection only supports linear searching.
            // Build dictionary for faster lookups.
            var lookup = StatusCollection
                .Where(status => status.Id != DonateNagStatus.DonateNagStatusId)
                .ToDictionary(status => status.Id, status => status);

            foreach (var status in statuses.Reverse())
            {
                if (lookup.TryGetValue(status.Id, out var fromCollection))
                {
                    if (status.OriginatingStatus != null)
                    {
                        fromCollection.OriginatingStatus?.UpdateFromStatus(status.OriginatingStatus);
                    }
                }
                else
                {
                    status.IsMyTweet = Settings.ScreenName == status.OriginatingStatus?.User?.ScreenName;
                    StatusCollection.Insert(0, status);
                }
            }
        }

        private void TruncateStatusCollection()
        {
            while (StatusCollection.Count > 500)
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