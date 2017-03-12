using System.Collections.Generic;

namespace RDeF.Collections
{
    internal static class CollectionExtensions
    {
        internal static void AddIf<T>(this ICollection<T> collection, T item, bool conditional)
        {
            if (conditional)
            {
                collection.Add(item);
            }
        }
    }
}
