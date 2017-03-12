using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using RDeF.Entities;

namespace RDeF.Mapping
{
    /// <summary>Represents an abstract mappings repository.</summary>
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "The type is not considered a collection.")]
    public interface IMappingsRepository : IEnumerable<IEntityMapping>
    {
        /// <summary>Finds the mapping for a given <paramref name="class" />.</summary>
        /// <param name="class">Class for which to obtain a mapping.</param>
        /// <returns>Matching <see cref="IEntityMapping" />.</returns>
        IEntityMapping FindEntityMappingFor(Iri @class);

        /// <summary>Finds the mapping for a given <paramref name="type" />.</summary>
        /// <param name="type">Type for which to obtain a mapping.</param>
        /// <returns>Matching <see cref="IEntityMapping" />.</returns>
        IEntityMapping FindEntityMappingFor(Type type);

        /// <summary>Finds the mapping for a given <paramref name="property" />.</summary>
        /// <param name="property">The property for which to obtain a mapping.</param>
        /// <returns>Matching <see cref="IPropertyMapping" />.</returns>
        IPropertyMapping FindPropertyMappingFor(PropertyInfo property);

        /// <summary>Finds the mapping for a given <paramref name="predicate" />.</summary>
        /// <param name="predicate">The predicate for which to obtain a mapping.</param>
        /// <returns>Matching <see cref="IPropertyMapping" />.</returns>
        IPropertyMapping FindPropertyMappingFor(Iri predicate);
    }
}