using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace twitter.core.Models
{
    public class User : INotifyPropertyChanged
    {
        public static readonly User Empty = new();

        private bool isFollowing;
        private bool isFollowedBy;
        private string? memberSince;

        [JsonPropertyName("id_str")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("screen_name")]
        public string? ScreenName { get; set; }

        [JsonPropertyName("profile_image_url_https")]
        public string? ProfileImageUrl { get; set; }

        [JsonPropertyName("profile_banner_url")]
        public string? ProfileBannerUrl { get; set; }

        [JsonIgnore]
        public string? ProfileBannerUrlSmall => string.IsNullOrWhiteSpace(ProfileBannerUrl)
            ? null
            : ProfileBannerUrl + "/300x100";

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("verified")]
        public bool Verified { get; set; }

        [JsonPropertyName("location")]
        public string? Location { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("statuses_count")]
        public int Tweets { get; set; }

        [JsonPropertyName("friends_count")]
        public int Friends { get; set; }

        [JsonPropertyName("followers_count")]
        public int Followers { get; set; }

        [JsonPropertyName("entities")]
        public UserObjectEntities? Entities { get; set; }

        [JsonPropertyName("created_at")]
        public string? CreatedAt { get; set; }

        [JsonPropertyName("following")]
        public bool IsFollowing { get => isFollowing; set => SetProperty(ref isFollowing, value); }

        [JsonPropertyName("followed_by")]
        public bool IsFollowedBy { get => isFollowedBy; set => SetProperty(ref isFollowedBy, value); }

        [JsonIgnore]
        public string MemberSince
        {
            get
            {
                if (memberSince is null)
                {
                    var date = TwitterStatus.ParseTwitterDate(CreatedAt);
                    memberSince = date.ToString("MMM yyy", CultureInfo.InvariantCulture);
                }
                return memberSince;
            }
        }

        [JsonIgnore]
        public string? ProfileImageUrlOriginal => ProfileImageUrl?.Replace("_normal", "", StringComparison.Ordinal);

        [JsonIgnore]
        public string? ProfileImageUrlBigger => ProfileImageUrl?.Replace("_normal", "_bigger", StringComparison.Ordinal);

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void SetProperty<T>(ref T item, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(item, value)) return;
            item = value;
            OnPropertyChanged(propertyName);
        }

        protected virtual void OnPropertyChanged(string? propertyName)
        {
            if (propertyName is null) return;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override bool Equals(object? obj)
        {
            return obj is User user && Id.Equals(user.Id, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            return StringComparer.Ordinal.GetHashCode(Id);
        }

        public override string? ToString()
        {
            return Name;
        }
    }
}