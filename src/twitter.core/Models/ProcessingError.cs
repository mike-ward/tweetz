using System.Text.Json.Serialization;

namespace twitter.core.Models
{
    public class ProcessingError
    {
        [JsonPropertyName("code")]
        public double Code { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}