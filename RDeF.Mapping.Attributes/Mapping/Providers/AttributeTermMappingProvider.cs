using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using RDeF.Entities;
using RDeF.Mapping.Visitors;

namespace RDeF.Mapping.Providers
{
    /// <summary>Represents an abstract attribute based property mapping provider.</summary>
    public abstract class AttributeTermMappingProvider : ITermMappingProvider
    {
        internal AttributeTermMappingProvider(Type entityType, Iri iri, Iri graph = null)
        {
            EntityType = entityType;
            Iri = iri;
            Graph = graph;
        }

        internal AttributeTermMappingProvider(Type entityType, string prefix, string term, Iri graph = null)
        {
            EntityType = entityType;
            Prefix = prefix;
            Term = term;
            Graph = graph;
        }

        internal AttributeTermMappingProvider(Type entityType, string prefix, string term, string graphPrefix = null, string graphTerm = null)
        {
            EntityType = entityType;
            Prefix = prefix;
            Term = term;
            GraphPrefix = graphPrefix;
            GraphTerm = graphTerm;
        }

        /// <inheritdoc />
        public Type EntityType { get; }

        /// <summary>Gets the mapped iri.</summary>
        protected Iri Iri { get; }

        /// <summary>Gets the mapped iri prefix.</summary>
        protected string Prefix { get; }

        /// <summary>Gets the mapped iri term.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Justification = "Protected and public members won't collide.")]
        protected string Term { get; }

        /// <summary>Gets the required graph.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Justification = "Protected and public members won't collide.")]
        protected Iri Graph { get; }

        /// <summary>Gets the mapped iri prefix.</summary>
        protected string GraphPrefix { get; }

        /// <summary>Gets the mapped iri term.</summary>
        protected string GraphTerm { get; }

        /// <inheritdoc />
        public Iri GetGraph(IEnumerable<QIriMapping> qiriMappings)
        {
            return qiriMappings.Resolve(Graph, GraphPrefix, GraphTerm);
        }

        /// <inheritdoc />
        public Iri GetTerm(IEnumerable<QIriMapping> qiriMappings)
        {
            return qiriMappings.Resolve(Iri, Prefix, Term);
        }

        /// <inheritdoc />
        public abstract void Accept(IMappingProviderVisitor visitor);

        /// <summary>Adds a given term mapping to the <paramref name="types" /> and <paramref name="parameters" />.</summary>
        /// <param name="types">Types for constructor.</param>
        /// <param name="parameters">Parameters for constructor.</param>
        /// <param name="iri">Iri being used.</param>
        /// <param name="prefix">Prefix being used.</param>
        /// <param name="term">Term being used.</param>
        internal static void AddTerm(IList<Type> types, IList<object> parameters, Iri iri, string prefix, string term)
        {
            if ((iri != null) || ((prefix == null) && (term == null)))
            {
                types.Add(typeof(Iri));
                parameters.Add(iri);
            }
            else
            {
                types.Add(typeof(string));
                types.Add(typeof(string));
                parameters.Add(prefix);
                parameters.Add(term);
            }
        }
    }
}
