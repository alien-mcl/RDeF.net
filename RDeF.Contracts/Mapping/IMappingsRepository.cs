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
        /// <param name="entity">Entity used for contextual mappings, if any.</param>
        /// <param name="class">Class for which to obtain a mapping.</param>
        /// <param name="graph">Optional named graph.</param>
        /// <returns>Matching <see cref="IEntityMapping" />.</returns>
        IEntityMapping FindEntityMappingFor(IEntity entity, Iri @class, Iri graph = null);

        /// <summary>Finds the mapping for a given <paramref name="entity" />s <paramref name="type" />.</summary>
        /// <param name="entity">Entity used for contextual mappings, if any.</param>
        /// <param name="type">Type for which to obtain a mapping.</param>
        /// <returns>Matching <see cref="IEntityMapping" />.</returns>
        IEntityMapping FindEntityMappingFor(IEntity entity, Type type);

        /// <summary>Finds the mapping for a given <typeparamref name="TEntity" />.</summary>
        /// <typeparam name="TEntity">Type for which to obtain a mapping.</typeparam>
        /// <param name="entity">Entity determining the type for which to obtain a mapping.</param>
        /// <returns>Matching <see cref="IEntityMapping" />.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "This is a part of a fluent-like API and works as intended.")]
        IEntityMapping FindEntityMappingFor<TEntity>(TEntity entity) where TEntity : IEntity;

        /// <summary>Finds the mappings for a given <paramref name="predicate" />.</summary>
        /// <param name="entity">Entity used for contextual mappings, if any.</param>
        /// <param name="predicate">The predicate for which to obtain a mapping.</param>
        /// <param name="graph">Optional named graph.</param>
        /// <returns>Matching <see cref="IPropertyMapping" />s.</returns>
        IEnumerable<IPropertyMapping> FindPropertyMappingsFor(IEntity entity, Iri predicate, Iri graph = null);

        /// <summary>Finds the mapping for a given <paramref name="predicate" />.</summary>
        /// <param name="entity">Entity used for contextual mappings, if any.</param>
        /// <param name="predicate">The predicate for which to obtain a mapping.</param>
        /// <param name="graph">Optional named graph.</param>
        /// <returns>Matching <see cref="IPropertyMapping" />.</returns>
        IPropertyMapping FindPropertyMappingFor(IEntity entity, Iri predicate, Iri graph = null);

        /// <summary>Finds the mapping for a given <paramref name="property" />.</summary>
        /// <param name="entity">Entity used for contextual mappings, if any.</param>
        /// <param name="property">The property for which to obtain a mapping.</param>
        /// <returns>Matching <see cref="IPropertyMapping" />.</returns>
        IPropertyMapping FindPropertyMappingFor(IEntity entity, PropertyInfo property);
    }
}