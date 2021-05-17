using System.Text.Json.Serialization;

namespace twitter.core.Models
{
    public class UserObjectEntities
    {
        [JsonPropertyName("url")] public UserObjectUrls? Url { get; set; }
    }
}