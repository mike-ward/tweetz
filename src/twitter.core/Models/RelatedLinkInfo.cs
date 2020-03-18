using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace twitter.core.Models
{
    /// <summary>
    /// When links are included in a tweet and it's not a quote or retweet,
    /// look for og/twitter meta tags in the links. This info, if present,
    /// is displayed similar to quotes.
    /// </summary>
    public class RelatedLinkInfo
    {
        public string Url { get; private set; }
        public string Title { get; private set; }
        public string? ImageUrl { get; private set; }

        public string Description { get; private set; }
        public string SiteName { get; private set; }

        public TwitterStatus ImageTwitterStatus
        {
            get
            {
                return new TwitterStatus
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
                            ExpandedUrl = ImageUrl
                        }
    }
                    }
                };
            }
        }

        public static async Task<RelatedLinkInfo?> GetRelatedLinkInfoAsync(TwitterStatus status)
        {
            if (status.IsQuoted)
            {
                status.CheckedRelatedInfo = true;
                return status.RelatedLinkInfo;
            }

            var urls = status.Entities?.Urls;
            if (urls is null)
            {
                status.CheckedRelatedInfo = true;
                return status.RelatedLinkInfo;
            }

            var hasMedia = status.OriginatingStatus.ExtendedEntities?.HasMedia ?? false;

            foreach (var url in urls)
            {
                try
                {
                    var uri = url.ExpandedUrl ?? url.Url;
                    if (!UrlValid(uri)) continue;
                    var relatedLinkInfo = await ParseHeadersForLinkInfo(uri).ConfigureAwait(false);
                    if (relatedLinkInfo is null) continue;

                    status.CheckedRelatedInfo = true;

                    if (hasMedia || !UrlValid(relatedLinkInfo.ImageUrl))
                    {
                        relatedLinkInfo.ImageUrl = null;
                    }

                    return status.RelatedLinkInfo ?? relatedLinkInfo;
                }
                catch (Exception ex)
                {
                    Trace.TraceWarning(ex.Message);
                }
            }

            status.CheckedRelatedInfo = true;
            return status.RelatedLinkInfo;
        }

        private static async Task<RelatedLinkInfo?> ParseHeadersForLinkInfo(string url)
        {
            if (!UrlValid(url)) return null;

            var request = WebRequest.Create(url);
            var response = await request.GetResponseAsync().ConfigureAwait(false);

            using var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            var html = reader.ReadToEnd();

            // No need to parse the whole document, only intereated in head section
            // This reduces memory and GC pressure for HTML Agility Pack
            const string headTag = "</head>";
            var index = html.IndexOf(headTag, StringComparison.OrdinalIgnoreCase);
            if (index < 0) return null;
            var length = index + headTag.Length;
            var head = html.Substring(0, length) + "</html>";

            var document = new HtmlDocument();
            document.LoadHtml(head);

            var metaTags = document.DocumentNode.SelectNodes("//meta");
            var metaInfo = new RelatedLinkInfo { Url = url };

            if (metaTags != null)
            {
                foreach (var tag in metaTags)
                {
                    var tagName = tag.Attributes["name"];
                    var tagContent = tag.Attributes["content"];
                    var tagProperty = tag.Attributes["property"];

                    if (tagName != null && tagContent != null)
                    {
                        switch (tagName.Value.ToLower(CultureInfo.InvariantCulture))
                        {
                            case "title":
                                metaInfo.Title = WebUtility.HtmlDecode(tagContent.Value);
                                break;

                            case "description":
                                metaInfo.Description = WebUtility.HtmlDecode(tagContent.Value);
                                break;

                            case "twitter:title":
                                metaInfo.Title = string.IsNullOrEmpty(metaInfo.Title)
                                    ? WebUtility.HtmlDecode(tagContent.Value)
                                    : WebUtility.HtmlDecode(metaInfo.Title);
                                break;

                            case "twitter:description":
                                metaInfo.Description = string.IsNullOrEmpty(metaInfo.Description)
                                    ? WebUtility.HtmlDecode(tagContent.Value)
                                    : WebUtility.HtmlDecode(metaInfo.Description);
                                break;

                            case "twitter:image:src":
                                metaInfo.ImageUrl = string.IsNullOrEmpty(metaInfo.ImageUrl)
                                    ? tagContent.Value
                                    : metaInfo.ImageUrl;
                                break;

                            case "twitter:site":
                                metaInfo.SiteName = string.IsNullOrEmpty(metaInfo.SiteName)
                                    ? WebUtility.HtmlDecode(tagContent.Value)
                                    : WebUtility.HtmlDecode(metaInfo.SiteName);
                                break;
                        }
                    }
                    else if (tagProperty != null && tagContent != null)
                    {
                        switch (tagProperty.Value.ToLower(CultureInfo.InvariantCulture))
                        {
                            case "og:title":
                                metaInfo.Title = string.IsNullOrEmpty(metaInfo.Title)
                                    ? WebUtility.HtmlDecode(tagContent.Value)
                                    : WebUtility.HtmlDecode(metaInfo.Title);
                                break;

                            case "og:description":
                                metaInfo.Description = string.IsNullOrEmpty(metaInfo.Description)
                                    ? WebUtility.HtmlDecode(tagContent.Value)
                                    : WebUtility.HtmlDecode(metaInfo.Description);
                                break;

                            case "og:image":
                                metaInfo.ImageUrl = string.IsNullOrEmpty(metaInfo.ImageUrl)
                                    ? tagContent.Value
                                    : metaInfo.ImageUrl;
                                break;

                            case "og:site_name":
                                metaInfo.SiteName = string.IsNullOrEmpty(metaInfo.SiteName)
                                    ? WebUtility.HtmlDecode(tagContent.Value)
                                    : WebUtility.HtmlDecode(metaInfo.SiteName);
                                break;
                        }
                    }
                }
            }

            return !string.IsNullOrEmpty(metaInfo.Title) && !string.IsNullOrEmpty(metaInfo.Description)
                ? metaInfo
                : null;
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
    }
}