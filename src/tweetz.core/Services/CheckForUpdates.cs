using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using tweetz.core.Infrastructure;
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
            var timer = new DispatcherTimer { Interval = twoHours };
            timer.Tick += (_, __) => Check();
            timer.Start();
            Check();
        }

        public string Version { get => version; set => SetProperty(ref version, value); }

        private void Check()
        {
            // fire and forget pattern
            CheckAsync().ConfigureAwait(false);
        }

        private async ValueTask CheckAsync()
        {
            try
            {
                var url = new Uri($"https://mike-ward.net/tweetz-version.txt?{DateTime.Now.Ticks}");
                var request = WebRequest.Create(url);
                using var response = await request.GetResponseAsync().ConfigureAwait(false);
                using var stream = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                Version = await stream.ReadToEndAsync();
            }
            catch (Exception ex)
            {
                // eat it, non-critical
                Trace.TraceError(ex.Message);
            }
        }
    }
}