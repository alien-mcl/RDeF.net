using System.Collections.Generic;
using RDeF.Mapping.Providers;

namespace RDeF.Mapping.Visitors
{
    internal static class TermMappingProviderExtensions
    {
        internal static void Accept(this ITermMappingProvider termMappingProvider, IEnumerable<IMappingProviderVisitor> visitors)
        {
            foreach (var visitor in visitors)
            {
                termMappingProvider.Accept(visitor);
            }
        }
    }
}
