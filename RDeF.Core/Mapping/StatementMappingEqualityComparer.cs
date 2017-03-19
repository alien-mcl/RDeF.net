using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace RDeF.Mapping
{
    internal sealed class StatementMappingEqualityComparer : IEqualityComparer<IStatementMapping>
    {
        internal static readonly StatementMappingEqualityComparer Default = new StatementMappingEqualityComparer();

        private StatementMappingEqualityComparer()
        {
        }

        /// <inheritdoc />
        public bool Equals(IStatementMapping x, IStatementMapping y)
        {
            return ((ReferenceEquals(x, null)) && (ReferenceEquals(y, null))) || (ReferenceEquals(x, y)) ||
                ((!ReferenceEquals(x, null)) && (!ReferenceEquals(y, null)) && ((Equals(x.Term, y.Term)) && (Equals(x.Graph, y.Graph))));
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "This method shouldn't throw.")]
        public int GetHashCode(IStatementMapping obj)
        {
            return (obj == null ? 0 : obj.Term.GetHashCode() ^ obj.Graph?.GetHashCode() ?? 0);
        }
    }
}
