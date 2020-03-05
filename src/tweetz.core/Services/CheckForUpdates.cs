using System;
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

        public CheckForUpdates(VersionInfo versionInfo)
        {
            version = versionInfo.Version;
            var twoHours = TimeSpan.FromHours(2);
            var timer = new DispatcherTimer { Interval = twoHours };
            timer.Tick += Check;
            timer.Start();
            Check(null, EventArgs.Empty);
        }

        public string Version { get => version; set => SetProperty(ref version, value); }

        private void Check(object? sender, EventArgs args)
        {
            // fire and forget pattern
            CheckAsync().ConfigureAwait(false);
        }

        private async Task CheckAsync()
        {
            try
            {
                var url = $"https://mike-ward.net/tweetz-version.txt?{DateTime.Now.Ticks}";
                var request = WebRequest.Create(url);
                using var response = await request.GetResponseAsync();
                using var stream = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                Version = stream.ReadToEnd();
            }
            catch
            {
                // eat it, non-critical
            }
        }
    }
}