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
    }
}