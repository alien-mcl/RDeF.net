using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace RDeF.Mapping
{
    internal sealed class PropertyMappingEqualityComparer : IEqualityComparer<IPropertyMapping>
    {
        internal static readonly PropertyMappingEqualityComparer Default = new PropertyMappingEqualityComparer();

        private PropertyMappingEqualityComparer()
        {
        }

        /// <inheritdoc />
        public bool Equals(IPropertyMapping x, IPropertyMapping y)
        {
            return (((ReferenceEquals(x, null)) && (ReferenceEquals(y, null))) || (ReferenceEquals(x, y)) ||
                ((!ReferenceEquals(x, null)) && (!ReferenceEquals(y, null)) &&
                ((Equals(x.Predicate, y.Predicate)) && (Equals(x.Name, y.Name)) && (Equals(x.Graph, y.Graph)) &&
                ((Equals(x.ValueConverter, y.ValueConverter)) || 
                ((!ReferenceEquals(x.ValueConverter, null)) && (!ReferenceEquals(y.ValueConverter, null)) && (x.ValueConverter.Equals(y.ValueConverter)))))));
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "This method shouldn't throw.")]
        public int GetHashCode(IPropertyMapping obj)
        {
            return (obj == null ? 0 : obj.Predicate.GetHashCode() ^ obj.Name.GetHashCode() ^ obj.Graph?.GetHashCode() ?? 0 ^ obj.ValueConverter?.GetHashCode() ?? 0);
        }
    }
}
