using HtmlAgilityPack;
using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using twitter.core.Services;

namespace twitter.core.Models
{
    /// <summary>
    /// When links are included in a tweet and it's not a quote or retweet,
    /// look for og/twitter meta tags in the links. This info, if present,
    /// is displayed similar to quotes.
    /// </summary>
    public class RelatedLinkInfo
    {
        public string Url { get; private set; } = string.Empty;
        public string Title { get; private set; } = string.Empty;
        public string? ImageUrl { get; private set; }
        public string Description { get; private set; } = string.Empty;
        public string SiteName { get; private set; } = string.Empty;

        public TwitterStatus ImageTwitterStatus
        {
            get => new TwitterStatus
            {
                Id = Guid.NewGuid().ToString(),
                ExtendedEntities = new Entities
                {
                    Media = string.IsNullOrWhiteSpace(ImageUrl)
                        ? null
                        : new[]
                        {
                            new Media
                            {
                                Url = ImageUrl,
                                MediaUrl = ImageUrl,
                                DisplayUrl = ImageUrl,
                                ExpandedUrl = ImageUrl,
                            },
                        },
                },
            };
        }

        public static async ValueTask<RelatedLinkInfo?> GetRelatedLinkInfoAsync(TwitterStatus status)
        {
            if (status.IsQuoted)
            {
                return status.RelatedLinkInfo;
            }

            var urls = status.Entities?.Urls;
            if (urls is null)
            {
                return status.RelatedLinkInfo;
            }

            var hasMedia = status.OriginatingStatus.ExtendedEntities?.HasMedia ?? false;

            foreach (var url in urls)
            {
                try
                {
                    var uri = url.ExpandedUrl ?? url.Url;
                    if (!UrlValid(uri)) continue;
                    var relatedLinkInfo = await GetLinkInfoAsync(uri).ConfigureAwait(false);
                    if (relatedLinkInfo is null) continue;

                    if (hasMedia || !UrlValid(relatedLinkInfo.ImageUrl))
                    {
                        relatedLinkInfo.ImageUrl = null;
                    }

                    return status.RelatedLinkInfo ?? relatedLinkInfo;
                }
                catch (Exception ex)
                {
                    TraceService.Message(ex.Message);
                }
            }

            return status.RelatedLinkInfo;
        }

        private static async ValueTask<RelatedLinkInfo?> GetLinkInfoAsync(string url)
        {
            if (!UrlValid(url)) return null;

            var request = (HttpWebRequest)WebRequest.Create(url);
            using var response = await request.GetResponseAsync().ConfigureAwait(false);

            var htmlBuilder = new StringBuilder();
            using var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);

            while (true)
            {
                var line = await reader.ReadLineAsync().ConfigureAwait(false);
                if (line is null) break;
                htmlBuilder.AppendLine(line);

                // No need to parse the whole document, only interested in head section
                const string headCloseTag = "</head>";
                if (line.Contains(headCloseTag, StringComparison.OrdinalIgnoreCase)) break;
            }

            var metaInfo = ParseForSocialTags(url, $"{htmlBuilder}</html>");

            return !string.IsNullOrEmpty(metaInfo.Title) && !string.IsNullOrEmpty(metaInfo.Description)
                ? metaInfo
                : null;
        }

        private static RelatedLinkInfo ParseForSocialTags(string url, string html)
        {
            var document = new HtmlDocument();
            document.LoadHtml(html);

            var metaTags = document.DocumentNode.SelectNodes("//meta");
            var metaInfo = new RelatedLinkInfo { Url = url };

            if (metaTags is not null)
            {
                foreach (var tag in metaTags)
                {
                    var tagName = tag.Attributes["name"];
                    var tagContent = tag.Attributes["content"];
                    var tagProperty = tag.Attributes["property"];

                    if (tagName is not null && tagContent is not null)
                    {
                        switch (tagName.Value.ToLower(CultureInfo.InvariantCulture))
                        {
                            case "title":
                                metaInfo.Title = DecodeHtml(tagContent.Value) ?? string.Empty;
                                break;

                            case "description":
                                metaInfo.Description = DecodeHtml(tagContent.Value) ?? string.Empty;
                                break;

                            case "twitter:title":
                                metaInfo.Title = string.IsNullOrEmpty(metaInfo.Title)
                                    ? DecodeHtml(tagContent.Value) ?? string.Empty
                                    : DecodeHtml(metaInfo.Title) ?? string.Empty;
                                break;

                            case "twitter:description":
                                metaInfo.Description = string.IsNullOrEmpty(metaInfo.Description)
                                    ? DecodeHtml(tagContent.Value) ?? string.Empty
                                    : DecodeHtml(metaInfo.Description) ?? string.Empty;
                                break;

                            case "twitter:image:src":
                                metaInfo.ImageUrl = string.IsNullOrEmpty(metaInfo.ImageUrl)
                                    ? tagContent.Value
                                    : metaInfo.ImageUrl;
                                break;

                            case "twitter:site":
                                metaInfo.SiteName = string.IsNullOrEmpty(metaInfo.SiteName)
                                    ? DecodeHtml(tagContent.Value) ?? string.Empty
                                    : DecodeHtml(metaInfo.SiteName) ?? string.Empty;
                                break;
                        }
                    }
                    else if (tagProperty is not null && tagContent is not null)
                    {
                        switch (tagProperty.Value.ToLower(CultureInfo.InvariantCulture))
                        {
                            case "og:title":
                                metaInfo.Title = string.IsNullOrEmpty(metaInfo.Title)
                                    ? DecodeHtml(tagContent.Value) ?? string.Empty
                                    : DecodeHtml(metaInfo.Title) ?? string.Empty;
                                break;

                            case "og:description":
                                metaInfo.Description = string.IsNullOrEmpty(metaInfo.Description)
                                    ? DecodeHtml(tagContent.Value) ?? string.Empty
                                    : DecodeHtml(metaInfo.Description) ?? string.Empty;
                                break;

                            case "og:image":
                                metaInfo.ImageUrl = string.IsNullOrEmpty(metaInfo.ImageUrl)
                                    ? tagContent.Value
                                    : metaInfo.ImageUrl;
                                break;

                            case "og:site_name":
                                metaInfo.SiteName = string.IsNullOrEmpty(metaInfo.SiteName)
                                    ? DecodeHtml(tagContent.Value) ?? string.Empty
                                    : DecodeHtml(metaInfo.SiteName) ?? string.Empty;
                                break;
                        }
                    }
                }
            }

            return metaInfo;
        }

        private static bool UrlValid(string? source)
        {
            if (string.IsNullOrWhiteSpace(source)) return false;

            try
            {
                var url = new Uri(source);
                return url.IsWellFormedOriginalString();
            }
            catch
            {
                return false;
            }
        }

        private static string DecodeHtml(string text)
        {
            // Twice to handle sequences like: "&amp;mdash;"
            return WebUtility.HtmlDecode(WebUtility.HtmlDecode(text));
        }
    }
}