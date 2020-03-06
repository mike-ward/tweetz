namespace tweetz.core.Models
{
    public class FlowContentNode
    {
        public FlowContentNodeType FlowContentNodeType { get; }
        public string Text { get; }
        public string? Parameter { get; }

        public FlowContentNode(FlowContentNodeType flowContentNodeType, string text)
        {
            FlowContentNodeType = flowContentNodeType;
            Text = text;
        }
    }
}