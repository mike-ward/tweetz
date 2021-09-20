using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using tweetz.core.Commands;
using tweetz.core.Converters;
using tweetz.core.Interfaces;
using tweetz.core.Models;
using twitter.core.Models;

namespace tweetz.core.Services
{
    public static class FlowContentService
    {
        public static IEnumerable<Inline> FlowContentInlines(TwitterStatus twitterStatus, ISettings settings)
        {
            twitterStatus.FlowContent ??= FlowContentNodes(twitterStatus).ToArray();
            var nodes = ((FlowContentNodeType, string)[])twitterStatus.FlowContent;

            foreach (var (flowContentNodeType, text) in nodes)
            {
                switch (flowContentNodeType)
                {
                    case FlowContentNodeType.Text:
                        yield return Run(text);
                        break;

                    case FlowContentNodeType.Url:
                        yield return Link(text, settings);
                        break;

                    case FlowContentNodeType.Mention:
                        yield return Mention(text);
                        break;

                    case FlowContentNodeType.HashTag:
                        yield return Hashtag(text);
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
            var start         = 0;
            var twitterString = new TwitterString(twitterStatus.FullText ?? twitterStatus.Text ?? string.Empty);

            foreach (var item in FlowControlItems(twitterStatus.Entities ?? new Entities()))
            {
                if (item.Start >= start)
                {
                    var len  = item.Start - start;
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

        private static readonly ShortLinkOptionConverter LinkOptionConverter = new();

        private static InlineUIContainer Link(string link, ISettings settings)
        {
            const int maxDisplayLength = 150;

            var binding = new Binding(nameof(System.Windows.Documents.Run.Text)) {
                Path               = new PropertyPath(nameof(settings.ShortLinks)),
                Source             = settings,
                Converter          = LinkOptionConverter,
                ConverterParameter = link
            };

            var run = new Run();
            run.SetBinding(System.Windows.Documents.Run.TextProperty, binding);

            var hyperlink = new Hyperlink(run) {
                CommandParameter = link,
                ToolTip          = link
            };

            void OnHyperlinkOnClick(object sender, RoutedEventArgs e)
            {
                OpenLinkCommand.Command.Execute(link, target: null);
            }

            void OnHyperlinkOnLoaded(object sender, RoutedEventArgs e)
            {
                hyperlink.Click          += OnHyperlinkOnClick;
                hyperlink.ToolTipOpening += LongUrlService.HyperlinkToolTipOpeningHandler;
            }

            void OnHyperlinkOnUnloaded(object sender, RoutedEventArgs e)
            {
                hyperlink.Click          -= OnHyperlinkOnClick;
                hyperlink.ToolTipOpening -= LongUrlService.HyperlinkToolTipOpeningHandler;
                hyperlink.Loaded         -= OnHyperlinkOnLoaded;
                hyperlink.Unloaded       -= OnHyperlinkOnUnloaded;
            }

            hyperlink.Loaded   += OnHyperlinkOnLoaded;
            hyperlink.Unloaded += OnHyperlinkOnUnloaded;

            var textblock = new TextBlock(hyperlink) {
                MaxWidth     = maxDisplayLength,
                TextTrimming = TextTrimming.CharacterEllipsis
            };

            return new InlineUIContainer(textblock);
        }

        private static Run Mention(string text)
        {
            var run = new Run(ConvertXmlEntities("@" + text)) {
                Style = GetMentionStyle()
            };

            var gesture = new MouseBinding(ShowUserProfileCommand.Command, new MouseGesture(MouseAction.LeftClick)) {
                CommandParameter = text
            };

            run.InputBindings.Add(gesture);
            return run;
        }

        private static Style? mentionStyle;

        private static Style GetMentionStyle()
        {
            return mentionStyle ??= Application.Current.FindResource("TweetBlockTranslateStyle") as Style ?? new Style();
        }

        private static Hyperlink Hashtag(string text)
        {
            var tag = "#" + text;
            var hyperlink = new Hyperlink(new Run(tag)) {
                CommandParameter = tag
            };

            void OnHyperlinkOnClick(object sender, RoutedEventArgs e) => SearchCommand.Command.Execute(tag, target: null);
            void OnHyperlinkOnLoaded(object sender, RoutedEventArgs e) => hyperlink.Click += OnHyperlinkOnClick;

            void OnHyperlinkOnUnloaded(object sender, RoutedEventArgs e)
            {
                hyperlink.Click    -= OnHyperlinkOnClick;
                hyperlink.Loaded   -= OnHyperlinkOnLoaded;
                hyperlink.Unloaded -= OnHyperlinkOnUnloaded;
            }

            hyperlink.Loaded   += OnHyperlinkOnLoaded;
            hyperlink.Unloaded += OnHyperlinkOnUnloaded;
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