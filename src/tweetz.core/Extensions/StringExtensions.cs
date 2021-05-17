using System;
using System.Runtime.CompilerServices;

namespace tweetz.core.Extensions
{
    public static class StringExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEqualTo(this string? a, string? b)
        {
            return string.CompareOrdinal(a, b) == 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNotEqualTo(this string? a, string? b)
        {
            return string.CompareOrdinal(a, b) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEqualToIgnoreCase(this string? a, string? b)
        {
            return string.Compare(a, b, StringComparison.OrdinalIgnoreCase) == 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNotEqualToIgnoreCase(this string? a, string? b)
        {
            return string.Compare(a, b, StringComparison.OrdinalIgnoreCase) != 0;
        }

        public static string? Truncate(this string? source, int length)
        {
            if (source is not null && source.Length > length)
            {
                source = source[..length];
            }

            return source;
        }
    }
}