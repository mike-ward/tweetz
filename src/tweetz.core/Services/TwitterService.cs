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
            var tokens = await twitterApi.GetPin();
            return tokens;
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

        public async Task<IEnumerable<TwitterStatus>> GetHomeTimeline()
        {
            var result = await twitterApi.HomeTimeline();
            return result;
        }

        public async Task<IEnumerable<TwitterStatus>> GetMentionsTimeline(int count = 20)
        {
            var result = await twitterApi.MentionsTimeline(count);
            return result;
        }

        public async Task<IEnumerable<TwitterStatus>> GetFavoritesTimeline()
        {
            var result = await twitterApi.FavoritesTimeline();
            return result;
        }

        public async Task<User> UserInfo(string screenName)
        {
            var result = await twitterApi.UserInfo(screenName);
            return result;
        }

        public async Task<Tweet> Search(string query)
        {
            var result = await twitterApi.Search(query);
            return result;
        }

        public async Task RetweetStatus(string statusId)
        {
            await twitterApi.RetweetStatus(statusId);
        }

        public async Task UnretweetStatus(string statusId)
        {
            await twitterApi.UnretweetStatus(statusId);
        }

        public async Task CreateFavorite(string statusId)
        {
            await twitterApi.CreateFavorite(statusId);
        }

        public async Task DestroyFavorite(string statusId)
        {
            await twitterApi.DestroyFavorite(statusId);
        }

        public async Task Follow(string screenName)
        {
            await twitterApi.Follow(screenName);
        }

        public async Task Unfollow(string screenName)
        {
            await twitterApi.Unfollow(screenName);
        }

        public async Task<TwitterStatus> UpdateStatus(string text, string? replyToStatusId, string? attachmentUrl, string[]? mediaIds)
        {
            return await twitterApi.UpdateStatus(text, replyToStatusId, attachmentUrl, mediaIds);
        }

        public async Task<TwitterStatus> GetStatus(string statusId)
        {
            return await twitterApi.GetStatus(statusId);
        }

        public async Task<UploadMedia> UploadMediaInit(int totalBytes, string mediaType)
        {
            return await twitterApi.UploadMediaInit(totalBytes, mediaType);
        }

        public async Task UploadMediaAppend(string mediaId, int segmentIndex, byte[] data)
        {
            await twitterApi.UploadMediaAppend(mediaId, segmentIndex, data);
        }

        public async Task<UploadMedia> UploadMediaStatus(string mediaId)
        {
            return await twitterApi.UploadMediaStatus(mediaId);
        }

        public async Task<UploadMedia> UploadMediaFinalize(string mediaId)
        {
            return await twitterApi.UploadMediaFinalize(mediaId);
        }
    }
}