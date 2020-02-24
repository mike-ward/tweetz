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
            var timer = new DispatcherTimer { Interval = TimeSpan.FromHours(2) };
            timer.Tick += async (s, args) => await Check();
            timer.Start();
            Task.Run(Check);
        }

        public string Version { get => version; set => SetProperty(ref version, value); }

        private async Task Check()
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