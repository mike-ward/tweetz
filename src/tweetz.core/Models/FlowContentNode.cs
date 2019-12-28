namespace tweetz.core.Models
{
    public class FlowContentNode
    {
        public FlowContentNodeType FlowContentNodeType { get; private set; }
        public string Text { get; private set; }
        public string? Parameter { get; }

        public FlowContentNode(FlowContentNodeType flowContentNodeType, string text)
        {
            FlowContentNodeType = flowContentNodeType;
            Text = text;
        }
    }
}