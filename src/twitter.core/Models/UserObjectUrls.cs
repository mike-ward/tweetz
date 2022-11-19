using System.Text.Json.Serialization;

namespace twitter.core.Models
{
    public class UserObjectUrls
    {
        [JsonPropertyName("urls")]
        public UrlEntity[]? Urls { get; set; }
    }
}