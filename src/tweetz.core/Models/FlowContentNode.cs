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
    }
}