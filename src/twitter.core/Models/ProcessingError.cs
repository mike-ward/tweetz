using System.Text.Json.Serialization;

namespace twitter.core.Models
{
    public class ProcessingError
    {
        public static readonly ProcessingError Empty = new ProcessingError();

        [JsonPropertyName("code")]
        public double Code { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
    }
}