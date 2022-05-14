namespace System.Collections.Generic
{
    /// <summary>
    /// List扩展
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// 按顺序插入元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="index"></param>
        /// <param name="items"></param>
        public static void InsertRange<T>(this IList<T> source, int index, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                source.Insert(index++, item);
            }
        }
    }
}
