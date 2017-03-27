using System.Collections.Generic;
using System.Linq;
using RDeF.Vocabularies;

namespace RDeF.Entities
{
    internal static class StatementExtensions
    {
        internal static bool IsLinkedList(this Statement value, ICollection<IGrouping<Iri, Statement>> subjects)
        {
            return (value.Object == rdf.nil) ||
                   ((value.Object != null) && (value.Object.IsBlank) &&
                    (subjects.Any(subject => subject.Key == value.Object && subject.Any(statement => statement.Predicate == rdf.first))));
        }
    }
}
