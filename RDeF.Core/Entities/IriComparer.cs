using System;
using System.Collections.Generic;

namespace RDeF.Entities
{
    /// <summary>Compares two <see cref="Iri" />s.</summary>
    public class IriComparer : IEqualityComparer<Iri>, IComparer<Iri>
    {
        /// <summary>Exposes a default instance of the <see cref="IriComparer" />.</summary>
        public static readonly IriComparer Default = new IriComparer();

        private IriComparer()
        {
        }

        /// <inheritdoc />
        public int Compare(Iri x, Iri y)
        {
            if ((ReferenceEquals(x, null)) && ((ReferenceEquals(y, null))))
            {
                return 0;
            }

            if (ReferenceEquals(x, null))
            {
                return -1;
            }

            if (ReferenceEquals(y, null))
            {
                return 1;
            }

            return String.CompareOrdinal(x.ToString(), y.ToString());
        }

        /// <inheritdoc />
        public bool Equals(Iri x, Iri y)
        {
            return (ReferenceEquals(x, y)) ||
                   ((ReferenceEquals(x, null)) && (ReferenceEquals(y, null))) ||
                   ((!ReferenceEquals(x, null)) && (!ReferenceEquals(y, null)) && (x == y));
        }

        /// <inheritdoc />
        public int GetHashCode(Iri obj)
        {
            return obj?.GetHashCode() ?? 0;
        }
    }
}
