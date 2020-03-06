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
            const string consumerKey = "ZScn2AEIQrfC48Zlw";
            const string consumerSecret = "8gKdPBwUfZCQfUiyeFeEwVBQiV3q50wIOrIjoCxa2Q";

            twitterApi = new TwitterApi(consumerKey, consumerSecret);
            this.settings = settings;

            this.settings.PropertyChanged += (s, args) => twitterApi.AuthenticationTokens(
                this.settings.AccessToken,
                this.settings.AccessTokenSecret);
        }

        public Task<OAuthTokens> GetPin()
        {
            return twitterApi.GetPin();
        }

        public async Task AuthenticateWithPin(OAuthTokens requestTokens, string pin)
        {
            var access = await twitterApi.AuthenticateWithPin(requestTokens, pin);
            if (access != null)
            {
                settings.AccessToken = access.OAuthToken;
                settings.AccessTokenSecret = access.OAuthSecret;
                settings.ScreenName = access.ScreenName;
                settings.Save();
            }
        }

        public Task<IEnumerable<TwitterStatus>> GetHomeTimeline()
        {
            return twitterApi.HomeTimeline();
        }

        public Task<IEnumerable<TwitterStatus>> GetMentionsTimeline(int count = 20)
        {
            return twitterApi.MentionsTimeline(count);
        }

        public Task<IEnumerable<TwitterStatus>> GetFavoritesTimeline()
        {
            return twitterApi.FavoritesTimeline();
        }

        public Task<User> UserInfo(string screenName)
        {
            return twitterApi.UserInfo(screenName);
        }

        public Task<Tweet> Search(string query)
        {
            return twitterApi.Search(query);
        }

        public Task RetweetStatus(string statusId)
        {
            return twitterApi.RetweetStatus(statusId);
        }

        public Task UnretweetStatus(string statusId)
        {
            return twitterApi.UnretweetStatus(statusId);
        }

        public Task CreateFavorite(string statusId)
        {
            return twitterApi.CreateFavorite(statusId);
        }

        public Task DestroyFavorite(string statusId)
        {
            return twitterApi.DestroyFavorite(statusId);
        }

        public Task Follow(string screenName)
        {
            return twitterApi.Follow(screenName);
        }

        public Task Unfollow(string screenName)
        {
            return twitterApi.Unfollow(screenName);
        }

        public Task<TwitterStatus> UpdateStatus(string text, string? replyToStatusId, string? attachmentUrl, string[]? mediaIds)
        {
            return twitterApi.UpdateStatus(text, replyToStatusId, attachmentUrl, mediaIds);
        }

        public Task<TwitterStatus> GetStatus(string statusId)
        {
            return twitterApi.GetStatus(statusId);
        }

        public Task<UploadMedia> UploadMediaInit(int totalBytes, string mediaType)
        {
            return twitterApi.UploadMediaInit(totalBytes, mediaType);
        }

        public Task UploadMediaAppend(string mediaId, int segmentIndex, byte[] data)
        {
            return twitterApi.UploadMediaAppend(mediaId, segmentIndex, data);
        }

        public Task<UploadMedia> UploadMediaStatus(string mediaId)
        {
            return twitterApi.UploadMediaStatus(mediaId);
        }

        public Task<UploadMedia> UploadMediaFinalize(string mediaId)
        {
            return twitterApi.UploadMediaFinalize(mediaId);
        }
    }
}