using System.Collections.Generic;
using System.Threading.Tasks;
using twitter.core.Models;
using twitter.core.Services;

namespace tweetz.core.Interfaces
{
    public interface ITwitterService
    {
        ValueTask<OAuthTokens> GetPin();
        ValueTask AuthenticateWithPinAsync(OAuthTokens requestTokens, string pin);
        ValueTask<IEnumerable<TwitterStatus>> GetHomeTimeline();
        ValueTask<IEnumerable<TwitterStatus>> GetMentionsTimeline(int count = 20);
        ValueTask<IEnumerable<TwitterStatus>> GetFavoritesTimeline();
        ValueTask<User> UserInfo(string screenName);
        ValueTask<Tweet> Search(string query);
        ValueTask RetweetStatus(string statusId);
        ValueTask UnretweetStatus(string statusId);
        ValueTask CreateFavorite(string statusId);
        ValueTask DestroyFavorite(string statusId);
        ValueTask Follow(string screenName);
        ValueTask Unfollow(string screenName);
        ValueTask<TwitterStatus> UpdateStatus(string text, string? replyToStatusId, string? attachmentUrl, string[] mediaIds);
        ValueTask<TwitterStatus> GetStatus(string statusId);
        ValueTask<UploadMedia> UploadMediaInit(int totalBytes, string mediaType);
        ValueTask UploadMediaAppend(string mediaId, int segmentIndex, byte[] data);
        ValueTask<UploadMedia> UploadMediaStatus(string mediaId);
        ValueTask<UploadMedia> UploadMediaFinalize(string mediaId);
        ValueTask<IEnumerable<UserConnection>> GetFriendships(string[] ids);
        ValueTask<IEnumerable<TwitterStatus>> GetUserTimeline(string screenName);
    }
}