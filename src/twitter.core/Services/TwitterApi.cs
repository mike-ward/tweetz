using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using twitter.core.Models;

namespace twitter.core.Services
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded", Justification = "None")]
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
            var requestTokens = await TwitterTokenRequest.GetRequestToken(ConsumerKey, ConsumerSecret).ConfigureAwait(true);

            var url = "https://api.twitter.com/oauth/authenticate?oauth_token=" + requestTokens.OAuthToken;
            OpenUrlService.Open(url);
            return requestTokens;
        }

        public Task<OAuthTokens> AuthenticateWithPin(OAuthTokens tokens, string pin)
        {
            return TwitterTokenRequest.GetAccessToken(ConsumerKey, ConsumerSecret, tokens.OAuthToken!, tokens.OAuthSecret!, pin);
        }

        public void AuthenticateWithTokens(string? accessToken, string? accessTokenSecret)
        {
            oAuthApiRequest.AuthenticateWithTokens(accessToken, accessTokenSecret);
        }

        public Task<IEnumerable<TwitterStatus>> HomeTimeline()
        {
            return oAuthApiRequest
                .Get<IEnumerable<TwitterStatus>>("https://api.twitter.com/1.1/statuses/home_timeline.json",
                    TwitterOptions.Default());
        }

        public Task<IEnumerable<TwitterStatus>> MentionsTimeline(int count)
        {
            return oAuthApiRequest
                .Get<IEnumerable<TwitterStatus>>("https://api.twitter.com/1.1/statuses/mentions_timeline.json",
                    TwitterOptions.Default(count));
        }

        public Task<IEnumerable<TwitterStatus>> FavoritesTimeline()
        {
            return oAuthApiRequest
                .Get<IEnumerable<TwitterStatus>>("https://api.twitter.com/1.1/favorites/list.json",
                    TwitterOptions.Default());
        }

        public Task<User> UserInfo(string screenName)
        {
            return oAuthApiRequest
                .Get<User>("https://api.twitter.com/1.1/users/show.json",
                    new[]
                    {
                        TwitterOptions.IncludeEntities(),
                        TwitterOptions.ExtendedTweetMode(),
                        TwitterOptions.ScreenName(screenName)
                    });
        }

        public Task<Tweet> Search(string query)
        {
            return oAuthApiRequest
                .Get<Tweet>("https://api.twitter.com/1.1/search/tweets.json",
                    new[]
                    {
                        TwitterOptions.Count(100),
                        TwitterOptions.Query(query),
                        TwitterOptions.IncludeEntities(),
                        TwitterOptions.ExtendedTweetMode()
                    });
        }

        public Task RetweetStatus(string statusId)
        {
            return oAuthApiRequest
               .Post($"https://api.twitter.com/1.1/statuses/retweet/{statusId}.json",
                   Array.Empty<(string, string)>());
        }

        public Task UnretweetStatus(string statusId)
        {
            return oAuthApiRequest
                .Post($"https://api.twitter.com/1.1/statuses/unretweet/{statusId}.json",
                    Array.Empty<(string, string)>());
        }

        public Task CreateFavorite(string statusId)
        {
            return oAuthApiRequest
                .Post("https://api.twitter.com/1.1/favorites/create.json",
                     new[] { TwitterOptions.Id(statusId) });
        }

        public Task DestroyFavorite(string statusId)
        {
            return oAuthApiRequest
               .Post("https://api.twitter.com/1.1/favorites/destroy.json",
                  new[] { TwitterOptions.Id(statusId) });
        }

        public Task Follow(string screenName)
        {
            return oAuthApiRequest
               .Post("https://api.twitter.com/1.1/friendships/create.json",
                   new[] { TwitterOptions.ScreenName(screenName), });
        }

        public Task Unfollow(string screenName)
        {
            return oAuthApiRequest
               .Post("https://api.twitter.com/1.1/friendships/destroy.json",
                   new[] { TwitterOptions.ScreenName(screenName) });
        }

        public Task<TwitterStatus> UpdateStatus(string text, string? replyToStatusId, string? attachmentUrl, string[]? mediaIds)
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

            return oAuthApiRequest
                .Post<TwitterStatus>("https://api.twitter.com/1.1/statuses/update.json", parameters);
        }

        public Task<TwitterStatus> GetStatus(string statusId)
        {
            return oAuthApiRequest
                .Get<TwitterStatus>("https://api.twitter.com/1.1/statuses/show.json",
                    new[]
                    {
                        TwitterOptions.Id(statusId),
                        TwitterOptions.IncludeEntities(),
                        TwitterOptions.ExtendedTweetMode()
                    });
        }

        public const string UploadMediaUrl = "https://upload.twitter.com/1.1/media/upload.json";

        public Task<UploadMedia> UploadMediaInit(int totalBytes, string mediaType)
        {
            return oAuthApiRequest
                .Post<UploadMedia>(UploadMediaUrl,
                    new[]
                    {
                        TwitterOptions.Command("INIT"),
                        TwitterOptions.TotalBytes(totalBytes),
                        TwitterOptions.MediaType(mediaType)
                    });
        }

        public Task UploadMediaAppend(string mediaId, int segmentIndex, byte[] data)
        {
            return oAuthApiRequest
               .AppendMedia(mediaId, segmentIndex, data);
        }

        public Task<UploadMedia> UploadMediaStatus(string mediaId)
        {
            return oAuthApiRequest
                .Get<UploadMedia>(UploadMediaUrl,
                    new[]
                    {
                        TwitterOptions.Command("STATUS"),
                        TwitterOptions.MediaId(mediaId)
                    });
        }

        public Task<UploadMedia> UploadMediaFinalize(string mediaId)
        {
            return oAuthApiRequest
                .Post<UploadMedia>(UploadMediaUrl,
                    new[]
                    {
                        TwitterOptions.Command("FINALIZE"),
                        TwitterOptions.MediaId(mediaId)
                    });
        }
    }
}