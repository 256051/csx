﻿namespace System.Collections.Generic
{
    public static class EnumerableExtensions
    {
        public static string JoinAsString<T>(this IEnumerable<T> source, string separator)
        {
            return string.Join(separator, source);
        }
    }
}
