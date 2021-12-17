using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows.Threading;
using tweetz.core.Interfaces;
using tweetz.core.Models;

namespace tweetz.core.Services
{
    public class CheckForUpdates : NotifyPropertyChanged, ICheckForUpdates
    {
        private string version;

        public CheckForUpdates()
        {
            version = VersionInfo.Version;
            var twoHours = TimeSpan.FromHours(2);
            var timer    = new DispatcherTimer { Interval = twoHours };
            timer.Tick += Check;
            timer.Start();
            Check(this, EventArgs.Empty);
        }

        public string Version
        {
            get => version;
            private set => SetProperty(ref version, value);
        }

        [SuppressMessage("Usage", "VSTHRD100", MessageId = "Avoid async void methods")]
        private async void Check(object? sender, EventArgs e)
        {
            try
            {
                var uri = new Uri($"https://mike-ward.net/tweetz-version.txt?{DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture)}");
                Version = await App.MyHttpClient.GetStringAsync(uri).ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                // eat it, non-critical
                TraceService.Message(ex.Message);
            }
        }
    }
}