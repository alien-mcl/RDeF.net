using System.Reflection;
using RDeF.Entities;

namespace RDeF.Mapping.Reflection
{
    /// <summary>Describes an <see cref="Iri" /> mapped property info.</summary>
    public class ExplicitlyMappedPropertyInfo : WrappingPropertyInfo
    {
        private readonly int _hashCode;

        internal ExplicitlyMappedPropertyInfo(PropertyInfo wrappedPropertyInfo, Iri predicate, Iri graph = null) : base(wrappedPropertyInfo)
        {
            Predicate = predicate;
            Graph = graph;
            _hashCode = wrappedPropertyInfo.GetHashCode() ^ predicate.GetHashCode() ^ (graph?.GetHashCode() ?? 0);
        }

        internal Iri Predicate { get; private set; }

        internal Iri Graph { get; private set; }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return _hashCode;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            var otherExplicitelyMappedPropertyInfo = obj as ExplicitlyMappedPropertyInfo;
            if (otherExplicitelyMappedPropertyInfo == null)
            {
                return false;
            }

            return WrappedPropertyInfo.Equals(otherExplicitelyMappedPropertyInfo.WrappedPropertyInfo) &&
                   Predicate.Equals(otherExplicitelyMappedPropertyInfo.Predicate) &&
                   Equals(Graph, otherExplicitelyMappedPropertyInfo.Graph);
        }
    }
}
