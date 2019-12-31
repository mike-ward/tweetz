using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using twitter.core.Models;

namespace twitter.core.Services
{
    public class TwitterApi
    {
        private string ConsumerKey { get; }
        private string ConsumerSecret { get; }
        private readonly OAuthApiRequest oAuthApiRequest;

        public TwitterApi(string consumerKey, string consumerSecret)
        {
            ConsumerKey = consumerKey;
            ConsumerSecret = consumerSecret;
            oAuthApiRequest = new OAuthApiRequest(ConsumerKey, ConsumerSecret);
        }

        public async Task<OAuthTokens> GetPin()
        {
            var requestTokens = await TwitterTokenRequest.GetRequestToken(ConsumerKey, ConsumerSecret);
            var url = "https://api.twitter.com/oauth/authenticate?oauth_token=" + requestTokens.OAuthToken;
            OpenUrlService.Open(url);
            return requestTokens;
        }

        public async Task<OAuthTokens> AuthenticateWithPin(OAuthTokens tokens, string pin)
        {
            if (tokens == null || tokens.OAuthToken == null || tokens.OAuthSecret == null) throw new ArgumentNullException(nameof(tokens));
            if (string.IsNullOrWhiteSpace(pin)) throw new ArgumentNullException(nameof(pin));

            var accessTokens = await TwitterTokenRequest.GetAccessToken(ConsumerKey, ConsumerSecret, tokens.OAuthToken, tokens.OAuthSecret, pin);
            return accessTokens;
        }

        public void AuthenticateWithTokens(string? accessToken, string? accessTokenSecret)
        {
            oAuthApiRequest.AuthenticateWithTokens(accessToken, accessTokenSecret);
        }

        public async Task<TwitterStatus[]> HomeTimeline()
        {
            return await oAuthApiRequest.Get<TwitterStatus[]>(
                "https://api.twitter.com/1.1/statuses/home_timeline.json",
                TwitterOptions.Default());
        }

        public async Task<TwitterStatus[]> MentionsTimeline(int count)
        {
            return await oAuthApiRequest.Get<TwitterStatus[]>(
                "https://api.twitter.com/1.1/statuses/mentions_timeline.json",
                TwitterOptions.Default(count));
        }

        public async Task<TwitterStatus[]> FavoritesTimeline()
        {
            return await oAuthApiRequest.Get<TwitterStatus[]>(
                "https://api.twitter.com/1.1/favorites/list.json",
                TwitterOptions.Default());
        }

        public async Task<User> UserInfo(string screenName)
        {
            return await oAuthApiRequest.Get<User>(
                "https://api.twitter.com/1.1/users/show.json",
                new[]
                {
                    TwitterOptions.IncludeEntities(),
                    TwitterOptions.ExtendedTweetMode(),
                    TwitterOptions.ScreenName(screenName)
                });
        }

        public async Task<Tweet> Search(string query)
        {
            return await oAuthApiRequest.Get<Tweet>(
                "https://api.twitter.com/1.1/search/tweets.json",
                new[]
                {
                    TwitterOptions.Count(100),
                    TwitterOptions.Query(query),
                    TwitterOptions.IncludeEntities(),
                    TwitterOptions.ExtendedTweetMode()
                });
        }

        public async Task RetweetStatus(string statusId)
        {
            await oAuthApiRequest.Post(
                $"https://api.twitter.com/1.1/statuses/retweet/{statusId}.json",
                new (string, string)[0]);
        }

        public async Task UnretweetStatus(string statusId)
        {
            await oAuthApiRequest.Post(
                $"https://api.twitter.com/1.1/statuses/unretweet/{statusId}.json",
                new (string, string)[0]);
        }

        public async Task CreateFavorite(string statusId)
        {
            await oAuthApiRequest.Post(
                "https://api.twitter.com/1.1/favorites/create.json",
                new[] { TwitterOptions.Id(statusId) });
        }

        public async Task DestroyFavorite(string statusId)
        {
            await oAuthApiRequest.Post(
                "https://api.twitter.com/1.1/favorites/destroy.json",
                new[] { TwitterOptions.Id(statusId) });
        }

        public async Task Follow(string screenName)
        {
            await oAuthApiRequest.Post(
                "https://api.twitter.com/1.1/friendships/create.json",
                new[] { TwitterOptions.ScreenName(screenName), });
        }

        public async Task Unfollow(string screenName)
        {
            await oAuthApiRequest.Post(
                "https://api.twitter.com/1.1/friendships/destroy.json",
                new[] { TwitterOptions.ScreenName(screenName) });
        }

        public async Task<TwitterStatus> UpdateStatus(string text, string? replyToStatusId, string? attachmentUrl, string[]? mediaIds)
        {
            var parameters = new List<(string, string)>
            {
                TwitterOptions.Status(text),
                TwitterOptions.ExtendedTweetMode()
            };

            if (!string.IsNullOrEmpty(replyToStatusId))
            {
                parameters.Add(TwitterOptions.ReplyStatusId(replyToStatusId));
                parameters.Add(TwitterOptions.AutoPopulateReplyMetadata());
            }

            if (!string.IsNullOrEmpty(attachmentUrl))
            {
                parameters.Add(TwitterOptions.AttachmentUrl(attachmentUrl));
            }

            if (mediaIds != null && mediaIds.Length > 0)
            {
                parameters.Add(TwitterOptions.MediaIds(mediaIds));
            }

            return await oAuthApiRequest.Post<TwitterStatus>(
                "https://api.twitter.com/1.1/statuses/update.json",
                parameters);
        }

        public async Task<TwitterStatus> GetStatus(string statusId)
        {
            return await oAuthApiRequest.Get<TwitterStatus>(
                "https://api.twitter.com/1.1/statuses/show.json",
                new[]
                {
                    TwitterOptions.Id(statusId),
                    TwitterOptions.IncludeEntities(),
                    TwitterOptions.ExtendedTweetMode()
                });
        }

        public static string UploadMediaUrl = "https://upload.twitter.com/1.1/media/upload.json";

        public async Task<UploadMedia> UploadMediaInit(int totalBytes, string mediaType)
        {
            return await oAuthApiRequest.Post<UploadMedia>(
                UploadMediaUrl,
                new[]
                {
                    TwitterOptions.Command("INIT"),
                    TwitterOptions.TotalBytes(totalBytes),
                    TwitterOptions.MediaType(mediaType)
                }); ;
        }

        public async Task UploadMediaAppend(string mediaId, int segmentIndex, byte[] data)
        {
            await oAuthApiRequest.AppendMedia(mediaId, segmentIndex, data);
        }

        public async Task<UploadMedia> UploadMediaStatus(string mediaId)
        {
            return await oAuthApiRequest.Post<UploadMedia>(
                UploadMediaUrl,
                new[]
                {
                    TwitterOptions.Command("STATUS"),
                    TwitterOptions.MediaId(mediaId)
                });
        }

        public async Task<UploadMedia> UploadMediaFinalize(string mediaId)
        {
            return await oAuthApiRequest.Post<UploadMedia>(
                UploadMediaUrl,
                new[]
                {
                    TwitterOptions.Command("FINALIZE"),
                    TwitterOptions.MediaId(mediaId)
                });
        }
    }
}