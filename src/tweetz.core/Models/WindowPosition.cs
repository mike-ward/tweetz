using System;
using System.Diagnostics.CodeAnalysis;

namespace tweetz.core.Models
{
    public struct WindowPosition : IEquatable<WindowPosition>
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public bool Equals([AllowNull] WindowPosition other)
        {
            return (Left, Top, Width, Height) == (other.Left, other.Top, other.Width, other.Height);
        }

        public override bool Equals(object? obj)
        {
            return obj is WindowPosition wp && Equals(wp);
        }

        public override int GetHashCode()
        {
            return (Left, Top, Width, Height).GetHashCode();
        }

        public static bool operator ==(WindowPosition a, WindowPosition b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(WindowPosition a, WindowPosition b)
        {
            return !a.Equals(b);
        }
    }
}