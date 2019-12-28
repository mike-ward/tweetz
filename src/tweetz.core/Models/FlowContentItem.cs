namespace tweetz.core.Models
{
    public class FlowContentItem
    {
        public FlowContentNodeType FlowContentNodeType { get; set; }
        public string Text { get; set; } = string.Empty;
        public int Start { get; set; }
        public int End { get; set; }
    }
}