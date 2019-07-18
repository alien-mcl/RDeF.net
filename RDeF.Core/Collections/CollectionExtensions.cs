using System.Collections.Generic;

namespace RDeF.Collections
{
    internal static class CollectionExtensions
    {
        internal static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            if (collection != null && items != null)
            {
                foreach (var item in items)
                {
                    collection.Add(item);
                }
            }
        }
    }
}
