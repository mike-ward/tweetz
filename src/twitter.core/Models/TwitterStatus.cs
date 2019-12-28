using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace twitter.core.Models
{
    public class TwitterStatus : INotifyPropertyChanged, IEquatable<TwitterStatus>
    {
        private int replyCount;
        private int retweetCount;
        private int favoriteCount;
        private int quoteCount;
        private DateTime createdDate;
        private RelatedLinkInfo? relatedLinkInfo;
        private bool retweetedByMe;
        private bool favorited;
        private bool checkedRelatedInfo;
        private bool isMyTweet;

        [JsonPropertyName("id_str")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("full_text")]
        public string? FullText { get; set; }

        [JsonPropertyName("text")]
        public string? Text { get; set; }

        [JsonPropertyName("user")]
        public User? User { get; set; }

        [JsonPropertyName("created_at")]
        public string? CreatedAt { get; set; }

        [JsonPropertyName("entities")]
        public Entities? Entities { get; set; }

        [JsonPropertyName("extended_entities")]
        public Entities? ExtendedEntities { get; set; }

        public bool IsQuoted => QuotedStatus != null;

        [JsonPropertyName("quoted_status")]
        public TwitterStatus? QuotedStatus { get; set; }

        [JsonPropertyName("quote_count")]
        public int QuoteCount { get => quoteCount; set => SetProperty(ref quoteCount, value); }

        public bool IsRetweet => RetweetedStatus != null;

        [JsonPropertyName("retweeted_status")]
        public TwitterStatus? RetweetedStatus { get; set; }

        [JsonPropertyName("retweeted")]
        public bool RetweetedByMe { get => retweetedByMe; set => SetProperty(ref retweetedByMe, value); }

        [JsonPropertyName("retweet_count")]
        public int RetweetCount { get => retweetCount; set => SetProperty(ref retweetCount, value); }

        [JsonPropertyName("favorite_count")]
        public int FavoriteCount { get => favoriteCount; set => SetProperty(ref favoriteCount, value); }

        [JsonPropertyName("favorited")]
        public bool Favorited { get => favorited; set => SetProperty(ref favorited, value); }

        [JsonPropertyName("in_reply_to_status_id_str")]
        public string? InReplyToStatusId { get; set; }

        [JsonPropertyName("in_reply_to_user_id_str")]
        public string? InReplyToUserId { get; set; }

        [JsonPropertyName("in_reply_to_screen_name")]
        public string? InReplyToScreenName { get; set; }

        [JsonPropertyName("reply_count")]
        public int ReplyCount { get => replyCount; set => SetProperty(ref replyCount, value); }

        [JsonIgnore]
        public string? OverrideLink { get; set; }

        [JsonIgnore]
        public bool CheckedRelatedInfo { get => checkedRelatedInfo; set => SetProperty(ref checkedRelatedInfo, value); }

        [JsonIgnore]
        public RelatedLinkInfo? RelatedLinkInfo
        {
            get
            {
                if (relatedLinkInfo != null) return relatedLinkInfo;
                if (!CheckedRelatedInfo) Task.Run(async () => { RelatedLinkInfo = await RelatedLinkInfo.GetRelatedLinkInfo(this); });
                return null;
            }
            set
            {
                SetProperty(ref relatedLinkInfo, value);
            }
        }

        /// <summary>
        /// Originating status is what get's displayed
        /// </summary>
        [JsonIgnore]
        public TwitterStatus? OriginatingStatus => IsRetweet ? RetweetedStatus : this;

        /// <summary>
        /// Create a link to a twitter status
        /// </summary>
        [JsonIgnore]
        public string StatusLink => string.IsNullOrWhiteSpace(OverrideLink)
            ? $"https://twitter.com/{User?.ScreenName}/status/{Id}"
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
        /// Inicates if user is author of tweet
        /// </summary>
        public bool IsMyTweet { get => isMyTweet; set => SetProperty(ref isMyTweet, value); }

        public static DateTime ParseTwitterDate(string? s)
        {
            if (string.IsNullOrWhiteSpace(s)) return default;

            return DateTime.ParseExact(
                s,
                "ddd MMM dd HH:mm:ss zzz yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.AdjustToUniversal);
        }

        /// <summary>
        /// Update a status's counts from a newer status
        /// </summary>
        /// <param name="status"></param>
        public void UpdateFromStatus(TwitterStatus status)
        {
            if (Id == null) throw new InvalidOperationException("Status Id is null");

            if (!Id.Equals(status.Id, StringComparison.Ordinal)) return;
            ReplyCount = status.ReplyCount;
            RetweetCount = status.RetweetCount;
            FavoriteCount = status.FavoriteCount;
            QuoteCount = status.QuoteCount;
            RetweetedByMe = status.RetweetedByMe;
            Favorited = status.Favorited;
        }

        /// <summary>
        /// Tricks the UI into updating the time ago dates in the timeline
        /// </summary>
        public void InvokeUpdateTimeStamp()
        {
            PropertyChanged?.Invoke(OriginatingStatus, new PropertyChangedEventArgs(nameof(CreatedDate)));
        }

        // INotifyProperyChanged Implementation
        //
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void SetProperty<T>(ref T item, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(item, value)) return;
            item = value;

            OnPropertyChanged(propertyName);
        }

        protected virtual void OnPropertyChanged(string? propertyName)
        {
            if (propertyName == null) return;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // IEquatable Implementation
        //
        public bool Equals([AllowNull] TwitterStatus other)
        {
            return other == null
                ? false
                : other.Id == Id;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as TwitterStatus);
        }

        public override int GetHashCode()
        {
            return Id?.GetHashCode() ?? 0;
        }
    }
}