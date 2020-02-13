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

            this.settings.PropertyChanged += (s, args) => twitterApi.AuthenticateWithTokens(
                this.settings.AccessToken,
                this.settings.AccessTokenSecret);
        }

        public async Task<OAuthTokens> GetPin()
        {
            return await twitterApi.GetPin().ConfigureAwait(false);
        }

        public async Task AuthenticateWithPin(OAuthTokens requestTokens, string pin)
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

        public async Task<IEnumerable<TwitterStatus>> GetHomeTimeline()
        {
            return await twitterApi.HomeTimeline().ConfigureAwait(false);
        }

        public async Task<IEnumerable<TwitterStatus>> GetMentionsTimeline(int count = 20)
        {
            return await twitterApi.MentionsTimeline(count).ConfigureAwait(false);
        }

        public async Task<IEnumerable<TwitterStatus>> GetFavoritesTimeline()
        {
            return await twitterApi.FavoritesTimeline().ConfigureAwait(false);
        }

        public async Task<User> UserInfo(string screenName)
        {
            return await twitterApi.UserInfo(screenName).ConfigureAwait(false);
        }

        public async Task<Tweet> Search(string query)
        {
            return await twitterApi.Search(query).ConfigureAwait(false);
        }

        public async Task RetweetStatus(string statusId)
        {
            await twitterApi.RetweetStatus(statusId).ConfigureAwait(false);
        }

        public async Task UnretweetStatus(string statusId)
        {
            await twitterApi.UnretweetStatus(statusId).ConfigureAwait(false);
        }

        public async Task CreateFavorite(string statusId)
        {
            await twitterApi.CreateFavorite(statusId).ConfigureAwait(false);
        }

        public async Task DestroyFavorite(string statusId)
        {
            await twitterApi.DestroyFavorite(statusId).ConfigureAwait(false);
        }

        public async Task Follow(string screenName)
        {
            await twitterApi.Follow(screenName).ConfigureAwait(false);
        }

        public async Task Unfollow(string screenName)
        {
            await twitterApi.Unfollow(screenName).ConfigureAwait(false);
        }

        public async Task<TwitterStatus> UpdateStatus(string text, string? replyToStatusId, string? attachmentUrl, string[]? mediaIds)
        {
            return await twitterApi.UpdateStatus(text, replyToStatusId, attachmentUrl, mediaIds).ConfigureAwait(false);
        }

        public async Task<TwitterStatus> GetStatus(string statusId)
        {
            return await twitterApi.GetStatus(statusId).ConfigureAwait(false);
        }

        public async Task<UploadMedia> UploadMediaInit(int totalBytes, string mediaType)
        {
            return await twitterApi.UploadMediaInit(totalBytes, mediaType).ConfigureAwait(false);
        }

        public async Task UploadMediaAppend(string mediaId, int segmentIndex, byte[] data)
        {
            await twitterApi.UploadMediaAppend(mediaId, segmentIndex, data).ConfigureAwait(false);
        }

        public async Task<UploadMedia> UploadMediaStatus(string mediaId)
        {
            return await twitterApi.UploadMediaStatus(mediaId).ConfigureAwait(false);
        }

        public async Task<UploadMedia> UploadMediaFinalize(string mediaId)
        {
            return await twitterApi.UploadMediaFinalize(mediaId).ConfigureAwait(false);
        }
    }
}