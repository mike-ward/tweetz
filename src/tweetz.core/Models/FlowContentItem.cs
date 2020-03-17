using System;

namespace tweetz.core.Models
{
    public struct FlowContentItem
    {
        public FlowContentNodeType FlowContentNodeType { get; }
        public string Text { get; }
        public int Start { get; }
        public int End { get; }

        public FlowContentItem(FlowContentNodeType nodeType, string text, int start, int end)
        {
            FlowContentNodeType = nodeType;
            Text = text;
            Start = start;
            End = end;
        }

        public override bool Equals(object? obj)
        {
            return obj is FlowContentItem item &&
                   Text == item.Text &&
                   Start == item.Start &&
                   End == item.End;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Text, Start, End);
        }

        public static bool operator ==(FlowContentItem left, FlowContentItem right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(FlowContentItem left, FlowContentItem right)
        {
            return !(left == right);
        }
    }
}