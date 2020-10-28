using System;
using System.Linq;
using System.Text.Json.Serialization;

namespace twitter.core.Models
{
    public struct UserConnection
    {
        [JsonPropertyName("id_str")]
        public string Id { get; set; }

        [JsonPropertyName("connections")]
        public string[] Connections { get; set; }

        public bool IsFollowing { get => Connections?.Contains("following", StringComparer.Ordinal) ?? false; }
        public bool IsFollowedBy { get => Connections?.Contains("followed_by", StringComparer.Ordinal) ?? false; }

        public override bool Equals(object? obj)
        {
            return obj is UserConnection uc && Id.Equals(uc.Id, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            return StringComparer.Ordinal.GetHashCode(Id);
        }

        public override string? ToString()
        {
            return Id;
        }

        public static bool operator ==(UserConnection left, UserConnection right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(UserConnection left, UserConnection right)
        {
            return !(left == right);
        }
    }
}