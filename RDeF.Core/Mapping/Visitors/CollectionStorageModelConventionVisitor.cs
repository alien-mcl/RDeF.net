using System;
using RDeF.Mapping.Providers;
using RollerCaster.Reflection;

namespace RDeF.Mapping.Visitors
{
    /// <summary>Applies RDF collection storage model based on how the mapped property is defined.</summary>
    public class CollectionStorageModelConventionVisitor : IMappingProviderVisitor
    {
        /// <inheritdoc />
        public void Visit(ICollectionMappingProvider collectionMappingProvider)
        {
            if (collectionMappingProvider == null)
            {
                throw new ArgumentNullException(nameof(collectionMappingProvider));
            }

            if (collectionMappingProvider.StoreAs == CollectionStorageModel.Unspecified)
            {
                collectionMappingProvider.StoreAs = collectionMappingProvider.Property.PropertyType.IsAListType()
                    ? CollectionStorageModel.LinkedList
                    : CollectionStorageModel.Simple;
            }
        }

        /// <inheritdoc />
        public void Visit(IPropertyMappingProvider propertyMappingProvider)
        {
        }

        /// <inheritdoc />
        public void Visit(IDictionaryMappingProvider dictionaryMappingProvider)
        {
        }

        /// <inheritdoc />
        public void Visit(IEntityMappingProvider entityMappingProvider)
        {
        }
    }
}
