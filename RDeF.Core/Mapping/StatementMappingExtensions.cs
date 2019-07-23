using RDeF.Entities;

namespace RDeF.Mapping
{
    internal static class StatementMappingExtensions
    {
        internal static bool Matches(this IStatementMapping statementMapping, Iri graph)
        {
            return statementMapping.Graph == null || graph == null || statementMapping.Graph == graph;
        }
    }
}
