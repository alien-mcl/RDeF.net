using System.Collections.Generic;
using System.Reflection;

namespace RDeF.Mapping
{
    /// <summary>Describes an abstract mapping source provider.</summary>
    public interface IMappingSourceProvider
    {
        /// <summary>Gets mapping sources from a given <paramref name="assembly" />.</summary>
        /// <param name="assembly">Assembly for which to obtain mapping sources.</param>
        /// <returns>Collection of mapping sources.</returns>
        IEnumerable<IMappingSource> GetMappingSourcesFor(Assembly assembly);
    }
}
