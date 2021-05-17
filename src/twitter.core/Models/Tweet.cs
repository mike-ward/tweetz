using System;
using System.Text.Json.Serialization;

namespace twitter.core.Models
{
    public class Tweet
    {
        [JsonPropertyName("statuses")] public TwitterStatus[] Statuses { get; set; } = Array.Empty<TwitterStatus>();
    }
}