using System.Text.Json.Serialization;

namespace twitter.core.Models
{
    public class CurrentUserRetweet
    {
        [JsonPropertyName("id_str")]
        public string? Id { get; set; }
    }
}