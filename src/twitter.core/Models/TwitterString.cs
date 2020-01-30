using System;
using System.Text;

namespace twitter.core.Models
{
    /// <summary>
    /// Twitter indices are by Unicode codepoints, not characters.
    /// See: https://developer.twitter.com/en/docs/basics/counting-characters
    /// </summary>
    public class TwitterString
    {
        private const int bytesPerChar = 4;
        private readonly Memory<byte> bytes;
        private readonly Encoding encoder = Encoding.UTF32;

        public TwitterString(string text)
        {
            bytes = encoder.GetBytes(text.Normalize());
        }

        public string Substring(int start)
        {
            var count = (bytes.Length / bytesPerChar) - start;
            return Substring(start, count);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Major Code Smell", "S3928:Parameter names used into ArgumentException constructors should match an existing one ",
            Justification = "OutOfRange is the most descriptive")
        ]
        public string Substring(int start, int count)
        {
            var bstart = start * bytesPerChar;
            var bcount = count * bytesPerChar;

            if (bstart < 0) throw new ArgumentOutOfRangeException(nameof(bstart), "start less than 0");
            if (bstart > bytes.Length) throw new ArgumentOutOfRangeException(nameof(bstart), "start greater than length");
            if (bcount < 0) throw new ArgumentOutOfRangeException(nameof(bcount), "count less than 0");
            if (bstart + bcount > bytes.Length) throw new ArgumentOutOfRangeException(nameof(bcount), "start + count greater than length");

            var substring = encoder.GetString(bytes.Slice(bstart, bcount).Span);
            return substring;
        }
    }
}