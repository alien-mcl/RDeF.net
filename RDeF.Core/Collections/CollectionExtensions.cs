using System.Collections.Generic;

namespace RDeF.Collections
{
    internal static class CollectionExtensions
    {
        internal static bool AddIf<T>(this ICollection<T> collection, T item, bool conditional)
        {
            if (!conditional)
            {
                return false;
            }

            collection.Add(item);
            return true;
        }
    }
}
