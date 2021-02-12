using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using tweetz.core.Commands;
using tweetz.core.Models;
using tweetz.core.Views.UserProfileBlock;
using twitter.core.Models;

namespace tweetz.core.Services
{
    public static class FlowContentService
    {
        public static IEnumerable<Inline> FlowContentInlines(TwitterStatus twitterStatus)
        {
            var nodes = FlowContentNodes(twitterStatus);

            foreach (var node in nodes)
            {
                switch (node.FlowContentNodeType)
                {
                    case FlowContentNodeType.Text:
                        yield return Run(node.Text);
                        break;

                    case FlowContentNodeType.Url:
                        yield return Link(node.Text);
                        break;

                    case FlowContentNodeType.Mention:
                        yield return Mention(node.Text);
                        break;

                    case FlowContentNodeType.HashTag:
                        yield return Hashtag(node.Text);
                        break;

                    case FlowContentNodeType.Media:
                        // Media is handled else where
                        break;

                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        private static IEnumerable<(FlowContentNodeType FlowContentNodeType, string Text)> FlowContentNodes(TwitterStatus twitterStatus)
        {
            var start = 0;
            var twitterString = new TwitterString(twitterStatus.FullText ?? twitterStatus.Text ?? string.Empty);

            foreach (var item in FlowControlItems(twitterStatus.Entities ?? new Entities()))
            {
                if (item.Start >= start)
                {
                    var len = item.Start - start;
                    var text = twitterString.Substring(start, len);
                    yield return (FlowContentNodeType.Text, text);
                }

                yield return (item.FlowContentNodeType, item.Text);
                start = item.End;
            }

            yield return (FlowContentNodeType.Text, twitterString.Substring(start));
        }

        private static IEnumerable<(FlowContentNodeType FlowContentNodeType, string Text, int Start, int End)> FlowControlItems(Entities entities)
        {
            var urls = entities.Urls
                 ?.Select(url =>
                 (
                     FlowContentNodeType: FlowContentNodeType.Url,
                     Text: url.ExpandedUrl,
                     Start: url.Indices[0],
                     End: url.Indices[1]
                 ))
                 ?? Enumerable.Empty<(FlowContentNodeType FlowContentNodeType, string Text, int Start, int End)>();

            var mentions = entities.Mentions
                ?.Select(mention =>
                (
                    FlowContentNodeType: FlowContentNodeType.Mention,
                    Text: mention.ScreenName,
                    Start: mention.Indices[0],
                    End: mention.Indices[1]
                ))
                ?? Enumerable.Empty<(FlowContentNodeType FlowContentNodeType, string Text, int Start, int End)>();

            var hashTags = entities.HashTags
                ?.Select(hashtag =>
                (
                    FlowContentNodeType: FlowContentNodeType.HashTag,
                    hashtag.Text,
                    Start: hashtag.Indices[0],
                    End: hashtag.Indices[1]
                ))
                ?? Enumerable.Empty<(FlowContentNodeType FlowContentNodeType, string Text, int Start, int End)>();

            var media = entities.Media
                ?.Select(entity =>
                (
                    FlowContentNodeType: FlowContentNodeType.Media,
                    Text: entity.Url,
                    Start: entity.Indices[0],
                    End: entity.Indices[1]
                ))
                ?? Enumerable.Empty<(FlowContentNodeType FlowContentNodeType, string Text, int Start, int End)>();

            return urls
                .Concat(mentions)
                .Concat(hashTags)
                .Concat(media)
                .OrderBy(o => o.Start);
        }

        private static Run Run(string text)
        {
            return new Run(ConvertXmlEntities(text));
        }

        private static InlineUIContainer Link(string link)
        {
            const int maxDisplayLength = 150;

            var hyperlink = new Hyperlink(new Run(link))
            {
                CommandParameter = link,
                ToolTip = link,
            };

            hyperlink.Click += delegate { OpenLinkCommand.Command.Execute(link, target: null); };
            hyperlink.ToolTipOpening += LongUrlService.HyperlinkToolTipOpeningHandler;

            var textblock = new TextBlock(hyperlink)
            {
                MaxWidth = maxDisplayLength,
                TextTrimming = TextTrimming.CharacterEllipsis,
            };

            return new InlineUIContainer(textblock);
        }

        private static Hyperlink Mention(string text)
        {
            var tooltip = new ToolTip();
            var userProfile = new UserProfileBlockControl();

            tooltip.Content = userProfile;
            tooltip.Style = GetToolTipStyle();
            userProfile.Tag = text;
            var link = $"https://twitter.com/{text}";

            var hyperlink = new Hyperlink(new Run("@" + text))
            {
                CommandParameter = link,
                ToolTip = tooltip,
            };

            hyperlink.Click += delegate { OpenLinkCommand.Command.Execute(link, target: null); };
            return hyperlink;
        }

        private static Style? userProfileToolTipStyle;

        private static Style GetToolTipStyle()
        {
            return userProfileToolTipStyle ??= ((Style)Application.Current.FindResource("ToolTipStyle"))!;
        }

        private static Hyperlink Hashtag(string text)
        {
            var tag = "#" + text;
            var hyperlink = new Hyperlink(new Run(tag))
            {
                CommandParameter = tag,
            };

            hyperlink.Click += delegate { SearchCommand.Command.Execute(tag, target: null); };
            return hyperlink;
        }

        private static string ConvertXmlEntities(string text)
        {
            // Twice to handle sequences like: "&amp;mdash;"
            return WebUtility.HtmlDecode(
                WebUtility.HtmlDecode(text));
        }
    }
}