using System.Text.Json.Serialization;

namespace twitter.core.Models
{
    public class UploadMedia
    {
        [JsonPropertyName("media_id_string")]
        public string? MediaId { get; set; }

        [JsonPropertyName("size")]
        public int Size { get; set; }

        [JsonPropertyName("expires_after_secs")]
        public int ExpiresAfterSecs { get; set; }

        [JsonPropertyName("processing_info")]
        public ProcessingInfo? ProcessingInfo { get; set; }
    }
}