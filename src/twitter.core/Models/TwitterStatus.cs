using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using twitter.core.Services;

namespace twitter.core.Models
{
    public class TwitterStatus : INotifyPropertyChanged
    {
        private int              replyCount;
        private int              retweetCount;
        private int              favoriteCount;
        private int              quoteCount;
        private DateTime         createdDate;
        private RelatedLinkInfo? relatedLinkInfo;
        private bool             retweetedByMe;
        private bool             favorited;
        private bool             isSensitive;
        private string?          translatedText;
        private string?          language;

        [JsonPropertyName("id_str")]
        public string Id { get; init; } = string.Empty;

        [JsonPropertyName("full_text")]
        public string? FullText { get; set; }

        [JsonPropertyName("text")]
        public string? Text { get; set; }

        [JsonPropertyName("user")]
        public User User { get; set; } = User.Empty;

        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; } = string.Empty;

        [JsonPropertyName("entities")]
        public Entities? Entities { get; set; }

        [JsonPropertyName("extended_entities")]
        public Entities? ExtendedEntities { get; set; }

        public bool IsQuoted => QuotedStatus is not null;

        [JsonPropertyName("quoted_status")]
        public TwitterStatus? QuotedStatus { get; set; }

        [JsonPropertyName("quote_count")]
        public int QuoteCount
        {
            get => quoteCount;
            set => SetProperty(ref quoteCount, value);
        }

        public bool IsRetweet => RetweetedStatus is not null;

        [JsonPropertyName("retweeted_status")]
        public TwitterStatus? RetweetedStatus { get; set; }

        [JsonPropertyName("retweeted")]
        public bool RetweetedByMe
        {
            get => retweetedByMe;
            set => SetProperty(ref retweetedByMe, value);
        }

        [JsonPropertyName("retweet_count")]
        public int RetweetCount
        {
            get => retweetCount;
            set => SetProperty(ref retweetCount, value);
        }

        [JsonPropertyName("favorite_count")]
        public int FavoriteCount
        {
            get => favoriteCount;
            set => SetProperty(ref favoriteCount, value);
        }

        [JsonPropertyName("favorited")]
        public bool Favorited
        {
            get => favorited;
            set => SetProperty(ref favorited, value);
        }

        [JsonPropertyName("in_reply_to_status_id_str")]
        public string? InReplyToStatusId { get; set; }

        [JsonPropertyName("in_reply_to_user_id_str")]
        public string? InReplyToUserId { get; set; }

        [JsonPropertyName("in_reply_to_screen_name")]
        public string? InReplyToScreenName { get; set; }

        [JsonPropertyName("reply_count")]
        public int ReplyCount
        {
            get => replyCount;
            set => SetProperty(ref replyCount, value);
        }

        [JsonPropertyName("possibly_sensitive")]
        public bool IsSensitive
        {
            get => isSensitive;
            set => SetProperty(ref isSensitive, value);
        }

        [JsonPropertyName("lang")]
        public string? Language
        {
            get => language;
            set => SetProperty(ref language, value);
        }

        [JsonIgnore]
        public object? FlowContent { get; set; }

        [JsonIgnore]
        public string? TranslatedText
        {
            get => translatedText;
            set => SetProperty(ref translatedText, value);
        }

        [JsonIgnore]
        public string? OverrideLink { get; set; }

        private int checkedRelatedInfo; // Interlocked.CompareExchange() does not support bool

        [JsonIgnore]
        [SuppressMessage("Usage", "VSTHRD105", MessageId = "Avoid method overloads that assume TaskScheduler.Current")]
        public RelatedLinkInfo? RelatedLinkInfo
        {
            get
            {
                if (Interlocked.CompareExchange(ref checkedRelatedInfo, value: 1, comparand: 0) == 0)
                {
                    var unused = Task.Factory.StartNew(async () => RelatedLinkInfo = await RelatedLinkInfo.GetRelatedLinkInfoAsync(this).ConfigureAwait(false));
                }

                return relatedLinkInfo;
            }
            set => SetProperty(ref relatedLinkInfo, value);
        }

        /// <summary>
        /// Originating status is what get's displayed
        /// </summary>
        [JsonIgnore]
        public TwitterStatus OriginatingStatus => IsRetweet
            ? RetweetedStatus ?? throw new InvalidOperationException("Invalid program state")
            : this;

        /// <summary>
        /// Create a link to a twitter status
        /// </summary>
        [JsonIgnore]
        public string StatusLink => string.IsNullOrWhiteSpace(OverrideLink)
            ? $"https://twitter.com/{User.ScreenName}/status/{Id}"
            : OverrideLink;

        /// <summary>
        /// Converts a serialized twitter date into a System.DateTime object and caches it.
        /// </summary>
        [JsonIgnore]
        public DateTime CreatedDate
        {
            get
            {
                if (createdDate == default)
                {
                    createdDate = ParseTwitterDate(CreatedAt);
                }

                return createdDate;
            }
        }

        /// <summary>
        /// Indicates if user is author of tweet
        /// </summary>
        public bool IsMyTweet { get; set; }

        public bool MentionsMe { get; set; }

        public static DateTime ParseTwitterDate(string? s)
        {
            return string.IsNullOrWhiteSpace(s)
                ? (default)
                : DateTime.ParseExact(
                    s,
                    "ddd MMM dd HH:mm:ss zzz yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AdjustToUniversal);
        }

        /// <summary>
        /// Update a status's counts from a newer status
        /// </summary>
        /// <param name="status"></param>
        public void UpdateFromStatus(TwitterStatus? status)
        {
            if (status is null) return;
            if (Id is null) throw new InvalidOperationException("Status Id is null");
            if (!Id.Equals(status.Id, StringComparison.Ordinal)) return;

            ReplyCount    = status.ReplyCount;
            RetweetCount  = status.RetweetCount;
            FavoriteCount = status.FavoriteCount;
            QuoteCount    = status.QuoteCount;
            RetweetedByMe = status.RetweetedByMe;
            Favorited     = status.Favorited;

            var userConnections = UserConnectionsService.LookupUserConnections(User.Id);
            User.IsFollowing  = userConnections?.IsFollowing ?? false;
            User.IsFollowedBy = userConnections?.IsFollowedBy ?? false;

            QuotedStatus?.UpdateFromStatus(status.QuotedStatus);
            RetweetedStatus?.UpdateFromStatus(status.RetweetedStatus);
        }

        public void UpdateAboutMeProperties(string? screenName)
        {
            if (string.IsNullOrEmpty(screenName)) return;
            IsMyTweet  = string.CompareOrdinal(screenName, OriginatingStatus.User.ScreenName) == 0;
            MentionsMe = Entities?.Mentions?.Any(mention => string.CompareOrdinal(mention.ScreenName, screenName) == 0) ?? false;
        }

        /// <summary>
        /// Tricks the UI into updating the time ago dates in the timeline
        /// </summary>
        public void InvokeUpdateTimeStamp()
        {
            PropertyChanged?.Invoke(OriginatingStatus, new PropertyChangedEventArgs(nameof(CreatedDate)));
        }

        // Overrides
        //

        public override bool Equals(object? obj)
        {
            return obj is TwitterStatus twitterStatus && Id.Equals(twitterStatus.Id, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            return StringComparer.Ordinal.GetHashCode(Id);
        }

        // INotifyPropertyChanged Implementation
        //
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void SetProperty<T>(ref T item, T value, [CallerMemberName] string? propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(item, value))
            {
                item = value;
                OnPropertyChanged(propertyName);
            }
        }

        protected void OnPropertyChanged(string? propertyName)
        {
            if (propertyName is not null && PropertyChanged is not null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}