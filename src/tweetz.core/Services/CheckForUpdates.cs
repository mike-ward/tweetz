using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
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

        private async void Check(object? sender, EventArgs e)
        {
            try
            {
                var url     = new Uri($"https://mike-ward.net/tweetz-version.txt?{DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture)}");
                var request = WebRequest.Create(url);

                using var response = await request.GetResponseAsync().ConfigureAwait(true);
                using var reader   = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                Version = await reader.ReadToEndAsync().ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                // eat it, non-critical
                TraceService.Message(ex.Message);
            }
        }
    }
}