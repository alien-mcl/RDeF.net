using RDeF.Entities;

namespace RDeF.Mapping
{
    /// <summary>Descibres an abstract statement mapping.</summary>
    public interface IStatementMapping
    {
        /// <summary>Gets the optional graph requirement by the mapping.</summary>
        Iri Graph { get; }

        /// <summary>Gets the predicate being mapped.</summary>
        Iri Term { get; }
    }
}
