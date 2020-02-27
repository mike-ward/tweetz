using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace twitter.core.Models
{
    public class TwitterStatusEqualityComparer : IEqualityComparer<TwitterStatus>
    {
        public bool Equals([AllowNull] TwitterStatus x, [AllowNull] TwitterStatus y)
        {
            return x != null && y != null && string.CompareOrdinal(x.Id, y.Id) == 0;
        }

        public int GetHashCode([DisallowNull] TwitterStatus obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}