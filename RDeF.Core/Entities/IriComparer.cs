using System;
using System.Collections.Generic;

namespace RDeF.Entities
{
    /// <summary>Compares two <see cref="Iri" />s.</summary>
    public class IriComparer : IComparer<Iri>
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
    }
}
