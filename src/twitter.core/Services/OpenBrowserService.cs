using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace twitter.core.Services
{
    public static class OpenUrlService
    {
        // Source: https://brockallen.com/2016/09/24/process-start-for-urls-on-net-core/
        //
        public static void Open(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&", StringComparison.Ordinal);
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw new InvalidOperationException($"Unrecoginized OSPlatform ({RuntimeInformation.OSDescription})");
                }
            }
        }
    }
}