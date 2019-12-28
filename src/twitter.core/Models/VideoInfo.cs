using System.Text.Json.Serialization;

namespace twitter.core.Models
{
    public class VideoInfo
    {
        [JsonPropertyName("variants")]
        public Variant[]? Variants { get; set; }
    }
}