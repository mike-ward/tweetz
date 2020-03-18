using System;
using System.Text;

namespace twitter.core.Models
{
    /// <summary>
    /// Twitter indices are by Unicode code points, not characters.
    /// See: https://developer.twitter.com/en/docs/basics/counting-characters
    /// </summary>
    public struct TwitterString
    {
        private const int bytesPerChar = 4;
        private readonly ReadOnlyMemory<byte> bytes;

        public TwitterString(string text)
        {
            bytes = Encoding.UTF32.GetBytes(text.Normalize());
        }

        public string Substring(int start)
        {
            var count = (bytes.Length / bytesPerChar) - start;
            return Substring(start, count);
        }

        public string Substring(int start, int count)
        {
            var bstart = start * bytesPerChar;
            var bcount = count * bytesPerChar;
            return Encoding.UTF32.GetString(bytes.Slice(bstart, bcount).Span);
        }
    }
}