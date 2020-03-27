using System.Collections.Generic;
using System.Threading.Tasks;
using twitter.core.Models;
using twitter.core.Services;

namespace tweetz.core.Infrastructure
{
    public interface ITwitterService
    {
        Task<OAuthTokens> GetPin();

        Task AuthenticateWithPin(OAuthTokens requestTokens, string pin);

        Task<IEnumerable<TwitterStatus>> GetHomeTimeline();

        Task<IEnumerable<TwitterStatus>> GetMentionsTimeline(int count = 20);

        Task<IEnumerable<TwitterStatus>> GetFavoritesTimeline();

        Task<User> UserInfo(string screenName);

        Task<Tweet> Search(string query);

        Task RetweetStatus(string statusId);

        Task UnretweetStatus(string statusId);

        Task CreateFavorite(string statusId);

        Task DestroyFavorite(string statusId);

        Task Follow(string screenName);

        Task Unfollow(string screenName);

        Task<TwitterStatus> UpdateStatus(string text, string? replyToStatusId, string? attachmentUrl, string[] mediaIds);

        Task<TwitterStatus> GetStatus(string statusId);

        Task<UploadMedia> UploadMediaInit(int totalBytes, string mediaType);

        Task UploadMediaAppend(string mediaId, int segmentIndex, byte[] data);

        Task<UploadMedia> UploadMediaStatus(string mediaId);

        Task<UploadMedia> UploadMediaFinalize(string mediaId);
    }
}