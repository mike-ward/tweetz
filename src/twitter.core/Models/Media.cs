using System;
using System.Text.Json.Serialization;

namespace twitter.core.Models
{
    public class Media
    {
        [JsonPropertyName("url")] public string Url { get; set; } = string.Empty;

        [JsonPropertyName("display_url")] public string DisplayUrl { get; set; } = string.Empty;

        [JsonPropertyName("expanded_url")] public string ExpandedUrl { get; set; } = string.Empty;

        [JsonPropertyName("media_url")] public string MediaUrl { get; set; } = string.Empty;

        [JsonPropertyName("indices")] public int[] Indices { get; set; } = Array.Empty<int>();

        [JsonPropertyName("video_info")] public VideoInfo? VideoInfo { get; set; }
    }
}