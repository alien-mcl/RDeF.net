using System.Collections.Generic;
using System.Linq;
using RollerCaster;

namespace RDeF.Entities
{
    internal static class MulticastPropertyValueExtensions
    {
        internal static MulticastPropertyValue FindMatching(this IEnumerable<MulticastPropertyValue> propertyValues, MulticastPropertyValue actualPropertyValue)
        {
            return propertyValues.FirstOrDefault(propertyValue => (propertyValue.Property == actualPropertyValue.Property) &&
                (propertyValue.CastedType == actualPropertyValue.CastedType));
        }
    }
}
