using System;
using System.Collections.Generic;
using System.Reflection;
using RDeF.Entities;

namespace RDeF.Mapping.Explicit
{
    /// <summary>Describes an abstract explicit mappings provider.</summary>
    public interface IExplicitMappings
    {
        /// <summary>Sets an <paramref name="entityMapping" />.</summary>
        /// <param name="entityMapping">Entity mapping to set.</param>
        /// <param name="owningEntity">Owning entity of the mappings.</param>
        void Set(IEntityMapping entityMapping, Iri owningEntity);

        /// <summary>Gets a explicitely set entity mapping.</summary>
        /// <param name="type">Type of the entity for which to obtain mapping.</param>
        /// <param name="owningEntity">Owning entity of the mappings.</param>
        /// <returns>Entity mapping matching a given <paramref name="type" /> or <b>null</b>.</returns>
        IEntityMapping FindEntityMappingFor(Type type, Iri owningEntity);

        /// <summary>Gets a explicitely set property mapping.</summary>
        /// <param name="predicate">Iri of the predicate for which to obtain mapping.</param>
        /// <param name="graph">Iri of the graph to match in mapping.</param>
        /// <param name="owningEntity">Owning entity of the mappings.</param>
        /// <returns>Property mapping matching a given <paramref name="predicate" /> or <b>null</b>.</returns>
        IPropertyMapping FindPropertyMappingFor(Iri predicate, Iri graph, Iri owningEntity);

        /// <summary>Gets a explicitely set property mapping.</summary>
        /// <param name="property">Property for which to obtain mapping.</param>
        /// <param name="owningEntity">Owning entity of the mappings.</param>
        /// <returns>Property mapping matching a given <paramref name="property" /> or <b>null</b>.</returns>
        IPropertyMapping FindPropertyMappingFor(PropertyInfo property, Iri owningEntity);

        /// <summary>Gets the enumerator of mappings.</summary>
        /// <returns>Instance of the <see cref="IEnumerator{IEntityMapping}" /> of mappings.</returns>
        IEnumerator<IEntityMapping> GetEnumerator();
    }
}
