using System;
using System.Collections.Generic;

namespace RDeF.Mapping
{
    /// <summary>Represents an entity mapping.</summary>
    public interface IEntityMapping
    {
        /// <summary>Gets the type being mapped.</summary>
        Type Type { get; }

        /// <summary>Gets the classes this entity is marked with.</summary>
        IEnumerable<IStatementMapping> Classes { get; }

        /// <summary>Gets the property mappings.</summary>
        IEnumerable<IPropertyMapping> Properties { get; }
    }
}
