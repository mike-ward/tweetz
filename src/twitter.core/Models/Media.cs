using System.Text.Json.Serialization;

namespace twitter.core.Models
{
    public class Media
    {
        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("display_url")]
        public string? DisplayUrl { get; set; }

        [JsonPropertyName("expanded_url")]
        public string? ExpandedUrl { get; set; }

        [JsonPropertyName("media_url")]
        public string? MediaUrl { get; set; }

        [JsonPropertyName("indices")]
        public int[]? Indices { get; set; }

        [JsonPropertyName("video_info")]
        public VideoInfo? VideoInfo { get; set; }
    }
}