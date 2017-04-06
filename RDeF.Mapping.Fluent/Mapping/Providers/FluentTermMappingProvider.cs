using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using RDeF.Entities;
using RDeF.Mapping.Visitors;

namespace RDeF.Mapping.Providers
{
    /// <summary>Represents an abstract fluent API based property mapping provider.</summary>
    public abstract class FluentTermMappingProvider : ITermMappingProvider
    {
        internal FluentTermMappingProvider(Type entityType, Iri iri, Iri graph = null)
        {
            EntityType = entityType;
            Iri = iri;
            Graph = graph;
        }

        internal FluentTermMappingProvider(Type entityType, string prefix, string term, Iri graph = null)
        {
            EntityType = entityType;
            Prefix = prefix;
            Term = term;
            Graph = graph;
        }

        internal FluentTermMappingProvider(Type entityType, string prefix, string term, string graphPrefix = null, string graphTerm = null)
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
            return Resolve(Graph, GraphPrefix, GraphTerm, qiriMappings);
        }

        /// <inheritdoc />
        public Iri GetTerm(IEnumerable<QIriMapping> qiriMappings)
        {
            return Resolve(Iri, Prefix, Term, qiriMappings);
        }

        /// <inheritdoc />
        public abstract void Accept(IMappingProviderVisitor visitor);

        private static Iri Resolve(Iri iri, string prefix, string term, IEnumerable<QIriMapping> qiriMappings)
        {
            if (iri != null)
            {
                return iri;
            }

            if ((prefix == null) || (term == null))
            {
                return null;
            }

            var result = (from qIriMapping in qiriMappings
                          where qIriMapping.Prefix == prefix
                          select qIriMapping.Iri).FirstOrDefault();
            if (result == null)
            {
                throw new InvalidOperationException($"Unable to resolve prefix '{prefix}'.");
            }

            return result + term;
        }
    }
}
