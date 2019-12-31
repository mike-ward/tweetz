using System.Text.Json.Serialization;

namespace twitter.core.Models
{
    public class HashTagEntity
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("indices")]
        public int[] Indices { get; set; }
    }
}