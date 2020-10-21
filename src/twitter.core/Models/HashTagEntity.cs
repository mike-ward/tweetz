using System;
using System.Text.Json.Serialization;

namespace twitter.core.Models
{
    public class HashTagEntity
    {
        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;

        [JsonPropertyName("indices")]
        public int[] Indices { get; set; } = Array.Empty<int>();
    }
}