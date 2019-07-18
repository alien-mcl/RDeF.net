using System.Collections.Generic;

namespace RDeF.Entities
{
    /// <summary>Describes an abstract entity.</summary>
    public interface IEntity
    {
        /// <summary>Gets the International Resource Identifier of this entity.</summary>
        Iri Iri { get; }

        /// <summary>Gets the resources that this entity is in through predicates that were unmapped to any of the properties.</summary>
        IEnumerable<Relation> UnmappedRelations { get; }

        /// <summary>Gets the entity context owning this entity.</summary>
        IEntityContext Context { get; }
    }
}
