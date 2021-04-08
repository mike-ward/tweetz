using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace tweetz.core.Services
{
    public static class LongUrlService
    {
        private const           int                                  maxCacheSize = 100;
        private static readonly ConcurrentDictionary<string, string> UrlCache     = new(concurrencyLevel: 1, capacity: maxCacheSize + 1, comparer: StringComparer.Ordinal);

        private static async ValueTask<string> TryGetLongUrlAsync(string link)
        {
            try
            {
                if (UrlCache.TryGetValue(link, out var longUrl))
                {
                    return longUrl;
                }

                const int FiveSeconds = 5000;
                using var tokenSource = new CancellationTokenSource(FiveSeconds);

                var       request  = new HttpRequestMessage { Method = HttpMethod.Head, RequestUri = new Uri(link) };
                using var response = await App.GetHttpClient().SendAsync(request, tokenSource.Token).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var uri = response.RequestMessage?.RequestUri?.AbsoluteUri;

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
                hyperlink.ToolTip = hyperlink.CommandParameter as string ?? string.Empty;
                hyperlink.ToolTip = await TryGetLongUrlAsync((string)hyperlink.ToolTip).ConfigureAwait(true);
            }
        }
    }
}