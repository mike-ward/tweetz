using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace tweetz.core.Services
{
    public static class LongUrlService
    {
        private const int maxCacheSize = 100;
        private static readonly ConcurrentDictionary<string, string> UrlCache = new ConcurrentDictionary<string, string>(concurrencyLevel: 1, capacity: maxCacheSize + 1, comparer: StringComparer.Ordinal);

        public static async ValueTask<string> TryGetLongUrlAsync(string link)
        {
            try
            {
                if (UrlCache.TryGetValue(link, out var longUrl))
                {
                    return longUrl;
                }

                var request = WebRequest.Create(new Uri(link));
                request.Method = "HEAD";
                const int timeoutInMilliseconds = 2000;
                request.Timeout = timeoutInMilliseconds;

                using var response = await request.GetResponseAsync().ConfigureAwait(false);
                var uri = response.ResponseUri.AbsoluteUri;

                if (!string.IsNullOrWhiteSpace(uri))
                {
                    if (UrlCache.Count > maxCacheSize)
                    {
                        UrlCache.Clear();
                    }

                    UrlCache.TryAdd(link, uri);
                    return uri;
                }
            }
            catch (Exception ex)
            {
                TraceService.Message(ex.Message);
            }
            return link;
        }

        public static async void HyperlinkToolTipOpeningHandler(object sender, ToolTipEventArgs args)
        {
            if (sender is Hyperlink hyperlink)
            {
                // Refresh tooltip now to prevent showing old link due to VirtualizingPanel.VirtualizationMode="Recycling"
                hyperlink.ToolTip = hyperlink.CommandParameter ?? string.Empty;

                await HyperlinkToolTipOpeningHandlerAsync(hyperlink).ConfigureAwait(false);
            }
        }

        private static async ValueTask HyperlinkToolTipOpeningHandlerAsync(Hyperlink hyperlink)
        {
            hyperlink.ToolTip = await TryGetLongUrlAsync((string)hyperlink.CommandParameter).ConfigureAwait(true);
        }
    }
}