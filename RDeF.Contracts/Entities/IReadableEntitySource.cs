using System.Collections.Generic;

namespace RDeF.Entities
{
    /// <summary>Describes an abstract RDF data source that can be read from.</summary>
    public interface IReadableEntitySource
    {
        /// <summary>Loads data related to a given resource identified with <paramref name="iri" />.</summary>
        /// <param name="iri">The identifier of the resource to load data for.</param>
        /// <returns>Set of statements related to resource identified with <paramref name="iri" />.</returns>
        IEnumerable<Statement> Load(Iri iri);
    }
}
