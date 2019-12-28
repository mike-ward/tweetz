using System.Linq;
using System.Text.Json.Serialization;

namespace twitter.core.Models
{
    public class Entities
    {
        [JsonPropertyName("urls")]
        public UrlEntity[]? Urls { get; set; }

        [JsonPropertyName("user_mentions")]
        public MentionEntity[]? Mentions { get; set; }

        [JsonPropertyName("hashtags")]
        public HashTagEntity[]? HashTags { get; set; }

        [JsonPropertyName("media")]
        public Media[]? Media { get; set; }

        [JsonIgnore]
        public bool HasMedia => Media != null && Media.All(media => string.IsNullOrWhiteSpace(media.MediaUrl));
    }
}