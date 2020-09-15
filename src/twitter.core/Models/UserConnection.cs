using System.Linq;
using System.Text.Json.Serialization;

namespace twitter.core.Models
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types")]
    public struct UserConnection
    {
        [JsonPropertyName("id_str")]
        public string Id { get; set; }

        [JsonPropertyName("connections")]
        public string[] Connections { get; set; }

        public bool IsFollowing { get => Connections?.Contains("following") ?? false; }
        public bool IsFollowedBy { get => Connections?.Contains("followed_by") ?? false; }
    }
}