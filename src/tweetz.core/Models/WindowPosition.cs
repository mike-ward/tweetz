using System;

namespace tweetz.core.Models
{
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct WindowPosition
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public override bool Equals(object? obj)
        {
            return
                obj is WindowPosition position
                && Left == position.Left
                && Top == position.Top
                && Width == position.Width
                && Height == position.Height;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Left, Top, Width, Height);
        }

        public static bool operator ==(WindowPosition left, WindowPosition right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(WindowPosition left, WindowPosition right)
        {
            return !(left == right);
        }
    }
}