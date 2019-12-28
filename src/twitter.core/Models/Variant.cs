using System.Text.Json.Serialization;

namespace twitter.core.Models
{
    public class Variant
    {
        [JsonPropertyName("url")]
        public string? Url { get; set; }
    }
}