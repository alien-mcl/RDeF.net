using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using RDeF.Entities;
using RDeF.Mapping.Attributes;
using RDeF.Mapping.Visitors;
#if NETSTANDARD1_6
using System.Linq;
#endif

namespace RDeF.Mapping.Providers
{
    /// <summary>Represents an attribute based collection mapping.</summary>
    [DebuggerDisplay("{EntityType.Name,nq}.{Property.Name,nq}[] ({Iri.ToString()??Prefix+\":\"+Term,nq})")]
    public sealed class AttributeCollectionMappingProvider : AttributePropertyMappingProvider, ICollectionMappingProvider
    {
        internal AttributeCollectionMappingProvider(
            Type entityType,
            PropertyInfo property,
            Iri iri,
            CollectionStorageModel storeAs = CollectionStorageModel.Unspecified,
            Type valueConverterType = null,
            Iri graph = null)
            : base(entityType, property, iri, valueConverterType, graph)
        {
            StoreAs = storeAs;
        }

        internal AttributeCollectionMappingProvider(
            Type entityType,
            PropertyInfo property,
            string prefix,
            string term,
            CollectionStorageModel storeAs = CollectionStorageModel.Unspecified,
            Type valueConverterType = null,
            Iri graph = null)
            : base(entityType, property, prefix, term, valueConverterType, graph)
        {
            StoreAs = storeAs;
        }

        internal AttributeCollectionMappingProvider(
            Type entityType,
            PropertyInfo property,
            string prefix,
            string term, 
            CollectionStorageModel storeAs = CollectionStorageModel.Unspecified,
            Type valueConverterType = null,
            string graphPrefix = null,
            string graphTerm = null)
            : base(entityType, property, prefix, term, valueConverterType, graphPrefix, graphTerm)
        {
            StoreAs = storeAs;
        }

        /// <inheritdoc />
        public CollectionStorageModel StoreAs { get; set; }

        /// <inheritdoc />
        public override void Accept(IMappingProviderVisitor visitor)
        {
            if (visitor == null)
            {
                throw new ArgumentNullException(nameof(visitor));
            }

            visitor.Visit(this);
        }

        internal static AttributeCollectionMappingProvider FromAttribute(Type type, PropertyInfo property, CollectionAttribute collectionMapping)
        {
            var types = new List<Type>() { typeof(Type), typeof(PropertyInfo) };
            var parameters = new List<object>() { type, property };
            AddTerm(types, parameters, collectionMapping.MappedIri, collectionMapping.Prefix, collectionMapping.Term);
            types.Add(typeof(CollectionStorageModel));
            parameters.Add(collectionMapping.StoreAs);
            types.Add(typeof(Type));
            parameters.Add(collectionMapping.ValueConverterType);
            AddTerm(types, parameters, collectionMapping.GraphIri, collectionMapping.GraphPrefix, collectionMapping.GraphTerm);
#if NETSTANDARD1_6
            return (AttributeCollectionMappingProvider)typeof(AttributeCollectionMappingProvider)
                .GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)
                .First(ctor => ctor.GetParameters().Select(parameter => parameter.ParameterType).SequenceEqual(types)).Invoke(parameters.ToArray());
#else
            return (AttributeCollectionMappingProvider)typeof(AttributeCollectionMappingProvider)
                .GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, types.ToArray(), null).Invoke(parameters.ToArray());
#endif
        }
    }
}
