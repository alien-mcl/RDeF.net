using System;
using System.Reflection;
using FluentAssertions;
using RDeF.Entities;
using RDeF.Mapping;
using RDeF.Mapping.Providers;

namespace RDeF
{
    internal static class AssertionsExtensions
    {
        internal static void MatchesMapped<TEntity>(this AttributeEntityMappingProvider provider, QIriMapping qiriMapping, Iri term, Iri graph = null)
        {
            var qiriMappings = (qiriMapping != null ? new[] { qiriMapping } : Array.Empty<QIriMapping>());
            provider.GetTerm(qiriMappings).Should().Be(term);
            provider.GetGraph(qiriMappings).Should().Be(graph);
            provider.EntityType.Should().Be(typeof(TEntity));
        }

        internal static void MatchesMapped<TEntity>(this AttributeEntityMappingProvider provider, Iri term, Iri graph = null)
        {
            provider.MatchesMapped<TEntity>(null, term, graph);
        }

        internal static void MatchesMapped<TConverter>(this AttributePropertyMappingProvider provider, PropertyInfo property, QIriMapping qiriMapping, Iri term, Iri graph = null)
        {
            provider.MatchesMapped(property, typeof(TConverter), qiriMapping, term, graph);
        }

        internal static void MatchesMapped<TConverter>(this AttributePropertyMappingProvider provider, PropertyInfo property, Iri term, Iri graph = null)
        {
            provider.MatchesMapped(property, typeof(TConverter), null, term, graph);
        }

        internal static void MatchesMapped<TConverter>(this AttributeCollectionMappingProvider provider, PropertyInfo property, QIriMapping qiriMapping, Iri term, Iri graph = null, CollectionStorageModel storageModel = CollectionStorageModel.Simple)
        {
            provider.MatchesMapped(property, typeof(TConverter), qiriMapping, term, graph).StoreAs.Should().Be(storageModel);
        }

        internal static void MatchesMapped<TConverter>(this AttributeCollectionMappingProvider provider, PropertyInfo property, Iri term, Iri graph = null, CollectionStorageModel storageModel = CollectionStorageModel.Simple)
        {
            provider.MatchesMapped(property, typeof(TConverter), null, term, graph).StoreAs.Should().Be(storageModel);
        }

        private static T MatchesMapped<T>(
            this T provider,
            PropertyInfo property,
            Type valueConverterType,
            QIriMapping qiriMapping,
            Iri term,
            Iri graph = null)
            where T : AttributePropertyMappingProvider
        {
            var qiriMappings = (qiriMapping != null ? new[] { qiriMapping } : Array.Empty<QIriMapping>());
            provider.GetTerm(qiriMappings).Should().Be(term);
            provider.GetGraph(qiriMappings).Should().Be(graph);
            provider.EntityType.Should().Be(property.DeclaringType);
            provider.Property.Should().BeSameAs(property);
            provider.ValueConverterType.Should().Be(valueConverterType);
            return provider;
        }
    }
}
