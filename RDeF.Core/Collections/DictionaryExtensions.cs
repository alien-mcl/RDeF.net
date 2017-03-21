using System.Collections.Generic;
using RollerCaster.Reflection;

namespace RDeF.Collections
{
    internal static class DictionaryExtensions
    {
        internal static TValue EnsureKey<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, bool concurrent = false)
        {
            TValue result = default(TValue);
            if (!dictionary.TryGetValue(key, out result))
            {
                dictionary[key] = result = (TValue)typeof(TValue)
                    .GetDefaultValue(concurrent ? CollectionOptions.Concurrent : CollectionOptions.None);
            }

            return result;
        }
    }
}
