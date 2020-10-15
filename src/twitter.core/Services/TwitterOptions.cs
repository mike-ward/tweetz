using System;
using System.Collections.Generic;
using System.Globalization;

namespace twitter.core.Services
{
    internal static class TwitterOptions
    {
        public static (string, string) Id(string Id) => ("id", Id);

        public static (string, string) Count(int count = 150) => ("count", count.ToString(CultureInfo.InvariantCulture));

        public static (string, string) SinceId(ulong sinceId) => ("since_id", sinceId.ToString(CultureInfo.InvariantCulture));

        public static (string, string) IncludeRetweets() => ("include_rts", "true");

        public static (string, string) IncludeEntities() => ("include_entities", "true");

        public static (string, string) ExtendedTweetMode() => ("tweet_mode", "extended");

        public static (string, string) ScreenName(string name) => ("screen_name", name);

        public static (string, string) Query(string query) => ("q", query);

        public static (string, string) Follow() => ("follow", "true");

        public static (string, string) Status(string text) => ("status", text);

        public static (string, string) ReplyStatusId(string id) => ("in_reply_to_status_id", id);

        public static (string, string) AttachmentUrl(string url) => ("attachment_url", url);

        public static (string, string) AutoPopulateReplyMetadata() => ("auto_populate_reply_metadata", "true");

        public static (string, string) Command(string command) => ("command", command);

        public static (string, string) TotalBytes(int size) => ("total_bytes", size.ToString(CultureInfo.InvariantCulture));

        public static (string, string) MediaType(string mediaType) => ("media_type", mediaType);

        public static (string, string) MediaId(string mediaId) => ("media_id", mediaId);

        public static (string, string) SegmentIndex(int index) => ("segment_index", index.ToString(CultureInfo.InvariantCulture));

        public static (string, string) MediaData(byte[] data) => ("media_data", Convert.ToBase64String(data));

        public static (string, string) MediaIds(IEnumerable<string> mediaIds) => ("media_ids", string.Join(',', mediaIds));

        public static (string, string) UserIds(IEnumerable<string> userIds) => ("user_id", string.Join(',', userIds));

        public static (string, string)[] Default(int count = 150) => new (string, string)[]
        {
            Count(count),
            IncludeRetweets(),
            IncludeEntities(),
            ExtendedTweetMode(),
        };
    }
}