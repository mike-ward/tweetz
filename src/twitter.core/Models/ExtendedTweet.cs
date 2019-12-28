using System.Text.Json.Serialization;

namespace twitter.core.Models
{
    public class ExtendedTweet
    {
        [JsonPropertyName("full_text")]
        public string? FullText { get; set; }

        [JsonPropertyName("entities")]
        public Entities? Entities { get; set; }

        [JsonPropertyName("extended_entities")]
        public Entities? ExtendedEntities { get; set; }
    }
}