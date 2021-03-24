namespace tweetz.core.Models
{
    public record WindowPosition
    {
        public int Left   { get; init; }
        public int Top    { get; init; }
        public int Width  { get; init; }
        public int Height { get; init; }
    }
}