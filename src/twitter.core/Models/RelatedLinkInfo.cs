using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
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
        public string  Url         { get; private set; } = string.Empty;
        public string  Title       { get; private set; } = string.Empty;
        public string? ImageUrl    { get; private set; }
        public string  Description { get; private set; } = string.Empty;
        public string  SiteName    { get; private set; } = string.Empty;
        public string  Language    { get; private set; } = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

        public TwitterStatus ImageTwitterStatus
        {
            get => new() {
                Id       = Guid.NewGuid().ToString(),
                Language = Language,
                FullText = Description,
                ExtendedEntities = new Entities {
                    Media = string.IsNullOrWhiteSpace(ImageUrl)
                        ? null
                        : new[] {
                            new Media {
                                Url         = ImageUrl,
                                MediaUrl    = ImageUrl,
                                DisplayUrl  = ImageUrl,
                                ExpandedUrl = ImageUrl
                            }
                        }
                }
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

            var       htmlBuilder = new StringBuilder();
            var       stream      = await OAuthApiRequest.MyHttpClient.GetStreamAsync(new Uri(url)).ConfigureAwait(false);
            using var reader      = new StreamReader(stream, Encoding.UTF8);

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

            var language = document.DocumentNode.SelectSingleNode("//html")?.Attributes["lang"]?.Value
                ?? CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

            var metaTags = document.DocumentNode.SelectNodes("//meta");
            var linkInfo = new RelatedLinkInfo { Url = url, Language = Truncate(language, 2) };

            if (metaTags is not null)
            {
                foreach (var tag in metaTags)
                {
                    var tagName     = tag.Attributes["name"];
                    var tagContent  = tag.Attributes["content"];
                    var tagProperty = tag.Attributes["property"];

                    if (tagName is not null && tagContent is not null)
                    {
                        switch (tagName.Value.ToLower(CultureInfo.InvariantCulture))
                        {
                            case "title":
                                linkInfo.Title = DecodeHtml(tagContent.Value);
                                break;

                            case "description":
                                linkInfo.Description = DecodeHtml(tagContent.Value);
                                break;

                            case "twitter:title":
                                linkInfo.Title = DecodeHtml(tagContent.Value);
                                break;

                            case "twitter:description":
                                linkInfo.Description = DecodeHtml(tagContent.Value);
                                break;

                            case "twitter:image:src":
                                linkInfo.ImageUrl = tagContent.Value;
                                break;

                            case "twitter:site":
                                linkInfo.SiteName = DecodeHtml(tagContent.Value);
                                break;
                        }
                    }
                    else if (tagProperty is not null && tagContent is not null)
                    {
                        switch (tagProperty.Value.ToLower(CultureInfo.InvariantCulture))
                        {
                            case "og:title":
                                linkInfo.Title = DecodeHtml(tagContent.Value);
                                break;

                            case "og:description":
                                linkInfo.Description = DecodeHtml(tagContent.Value);
                                break;

                            case "og:image":
                                linkInfo.ImageUrl = tagContent.Value;
                                break;

                            case "og:site_name":
                                linkInfo.SiteName = DecodeHtml(tagContent.Value);
                                break;
                        }
                    }
                }
            }

            return linkInfo;
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
            return WebUtility.HtmlDecode(WebUtility.HtmlDecode(text)) ?? string.Empty;
        }

        private static string Truncate(string source, int length)
        {
            if (source.Length > length)
            {
                source = source[..length];
            }

            return source;
        }
    }
}