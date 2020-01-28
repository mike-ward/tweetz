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
            var consumerKey = "ZScn2AEIQrfC48Zlw";
            var consumerSecret = "8gKdPBwUfZCQfUiyeFeEwVBQiV3q50wIOrIjoCxa2Q";

            twitterApi = new TwitterApi(consumerKey, consumerSecret);
            this.settings = settings;
            this.settings.PropertyChanged += (s, args) => twitterApi.AuthenticateWithTokens(this.settings.AccessToken, this.settings.AccessTokenSecret);
        }

        public async Task<OAuthTokens> GetPin()
        {
            var tokens = await twitterApi.GetPin().ConfigureAwait(true);
            return tokens;
        }

        public async Task AuthenticateWithPin(OAuthTokens requestTokens, string pin)
        {
            var access = await twitterApi.AuthenticateWithPin(requestTokens, pin).ConfigureAwait(true);
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
            var result = await twitterApi.HomeTimeline().ConfigureAwait(true);
            return result;
        }

        public async Task<IEnumerable<TwitterStatus>> GetMentionsTimeline(int count)
        {
            var result = await twitterApi.MentionsTimeline(count).ConfigureAwait(true);
            return result;
        }

        public async Task<IEnumerable<TwitterStatus>> GetFavoritesTimeline()
        {
            var result = await twitterApi.FavoritesTimeline().ConfigureAwait(true);
            return result;
        }

        public async Task<User> UserInfo(string screenName)
        {
            var result = await twitterApi.UserInfo(screenName).ConfigureAwait(true);
            return result;
        }

        public async Task<Tweet> Search(string query)
        {
            var result = await twitterApi.Search(query).ConfigureAwait(true);
            return result;
        }

        public async Task RetweetStatus(string statusId)
        {
            await twitterApi.RetweetStatus(statusId).ConfigureAwait(true);
        }

        public async Task UnretweetStatus(string statusId)
        {
            await twitterApi.UnretweetStatus(statusId).ConfigureAwait(true);
        }

        public async Task CreateFavorite(string statusId)
        {
            await twitterApi.CreateFavorite(statusId).ConfigureAwait(true);
        }

        public async Task DestroyFavorite(string statusId)
        {
            await twitterApi.DestroyFavorite(statusId).ConfigureAwait(true);
        }

        public async Task Follow(string screenName)
        {
            await twitterApi.Follow(screenName).ConfigureAwait(true);
        }

        public async Task Unfollow(string screenName)
        {
            await twitterApi.Unfollow(screenName).ConfigureAwait(true);
        }

        public async Task<TwitterStatus> UpdateStatus(string text, string? replyToStatusId, string? attachmentUrl, string[]? mediaIds)
        {
            return await twitterApi.UpdateStatus(text, replyToStatusId, attachmentUrl, mediaIds).ConfigureAwait(true);
        }

        public async Task<TwitterStatus> GetStatus(string statusId)
        {
            return await twitterApi.GetStatus(statusId).ConfigureAwait(true);
        }

        public async Task<UploadMedia> UploadMediaInit(int totalBytes, string mediaType)
        {
            return await twitterApi.UploadMediaInit(totalBytes, mediaType).ConfigureAwait(true);
        }

        public async Task UploadMediaAppend(string mediaId, int segmentIndex, byte[] data)
        {
            await twitterApi.UploadMediaAppend(mediaId, segmentIndex, data).ConfigureAwait(true);
        }

        public async Task<UploadMedia> UploadMediaStatus(string mediaId)
        {
            return await twitterApi.UploadMediaStatus(mediaId).ConfigureAwait(true);
        }

        public async Task<UploadMedia> UploadMediaFinalize(string mediaId)
        {
            return await twitterApi.UploadMediaFinalize(mediaId).ConfigureAwait(true);
        }
    }
}