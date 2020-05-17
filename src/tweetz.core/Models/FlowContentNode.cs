using System;

namespace tweetz.core.Models
{
    public struct FlowContentNode
    {
        public FlowContentNodeType FlowContentNodeType { get; }
        public string Text { get; }

        public FlowContentNode(FlowContentNodeType flowContentNodeType, string text)
        {
            FlowContentNodeType = flowContentNodeType;
            Text = text;
        }

        public override bool Equals(object? obj)
        {
            return obj is FlowContentNode node &&
                   FlowContentNodeType == node.FlowContentNodeType &&
                   Text.Equals(node.Text, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FlowContentNodeType, Text);
        }

        public static bool operator ==(FlowContentNode left, FlowContentNode right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(FlowContentNode left, FlowContentNode right)
        {
            return !(left == right);
        }
    }
}