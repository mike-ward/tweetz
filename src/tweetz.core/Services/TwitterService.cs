using System.Collections.Generic;
using System.Threading.Tasks;
using tweetz.core.Infrastructure;
using twitter.core.Models;
using twitter.core.Services;

namespace tweetz.core.Services
{
    public class TwitterService : ITwitterService
    {
        private readonly TwitterApi twitterApi;
        private readonly ISettings settings;

        public TwitterService(ISettings settings)
        {
            if (settings is null) throw new System.ArgumentNullException(nameof(settings));

            const string consumerKey = "ZScn2AEIQrfC48Zlw";
            const string consumerSecret = "8gKdPBwUfZCQfUiyeFeEwVBQiV3q50wIOrIjoCxa2Q";

            twitterApi = new TwitterApi(consumerKey, consumerSecret);
            this.settings = settings;

            this.settings.PropertyChanged += (s, args) => twitterApi.AuthenticationTokens(
                this.settings.AccessToken,
                this.settings.AccessTokenSecret);
        }

        public ValueTask<OAuthTokens> GetPin()
        {
            return twitterApi.GetPin();
        }

        public async ValueTask AuthenticateWithPin(OAuthTokens requestTokens, string pin)
        {
            var access = await twitterApi.AuthenticateWithPin(requestTokens, pin).ConfigureAwait(false);
            if (access != null)
            {
                settings.AccessToken = access.OAuthToken;
                settings.AccessTokenSecret = access.OAuthSecret;
                settings.ScreenName = access.ScreenName;
                settings.Save();
            }
        }

        public ValueTask<IEnumerable<TwitterStatus>> GetHomeTimeline()
        {
            return twitterApi.HomeTimeline();
        }

        public ValueTask<IEnumerable<TwitterStatus>> GetMentionsTimeline(int count = 20)
        {
            return twitterApi.MentionsTimeline(count);
        }

        public ValueTask<IEnumerable<TwitterStatus>> GetFavoritesTimeline()
        {
            return twitterApi.FavoritesTimeline();
        }

        public ValueTask<User> UserInfo(string screenName)
        {
            return twitterApi.UserInfo(screenName);
        }

        public ValueTask<Tweet> Search(string query)
        {
            return twitterApi.Search(query);
        }

        public ValueTask RetweetStatus(string statusId)
        {
            return twitterApi.RetweetStatus(statusId);
        }

        public ValueTask UnretweetStatus(string statusId)
        {
            return twitterApi.UnretweetStatus(statusId);
        }

        public ValueTask CreateFavorite(string statusId)
        {
            return twitterApi.CreateFavorite(statusId);
        }

        public ValueTask DestroyFavorite(string statusId)
        {
            return twitterApi.DestroyFavorite(statusId);
        }

        public ValueTask Follow(string screenName)
        {
            return twitterApi.Follow(screenName);
        }

        public ValueTask Unfollow(string screenName)
        {
            return twitterApi.Unfollow(screenName);
        }

        public ValueTask<TwitterStatus> UpdateStatus(string text, string? replyToStatusId, string? attachmentUrl, string[]? mediaIds)
        {
            return twitterApi.UpdateStatus(text, replyToStatusId, attachmentUrl, mediaIds);
        }

        public ValueTask<TwitterStatus> GetStatus(string statusId)
        {
            return twitterApi.GetStatus(statusId);
        }

        public ValueTask<UploadMedia> UploadMediaInit(int totalBytes, string mediaType)
        {
            return twitterApi.UploadMediaInit(totalBytes, mediaType);
        }

        public ValueTask UploadMediaAppend(string mediaId, int segmentIndex, byte[] data)
        {
            return twitterApi.UploadMediaAppend(mediaId, segmentIndex, data);
        }

        public ValueTask<UploadMedia> UploadMediaStatus(string mediaId)
        {
            return twitterApi.UploadMediaStatus(mediaId);
        }

        public ValueTask<UploadMedia> UploadMediaFinalize(string mediaId)
        {
            return twitterApi.UploadMediaFinalize(mediaId);
        }
    }
}