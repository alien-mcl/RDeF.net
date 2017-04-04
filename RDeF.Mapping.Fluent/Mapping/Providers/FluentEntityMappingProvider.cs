using System;
using System.Diagnostics;
using RDeF.Entities;
using RDeF.Mapping.Visitors;

namespace RDeF.Mapping.Providers
{
    /// <summary>Represents a fluent API based property mapping.</summary>
    [DebuggerDisplay("{EntityType.Name,nq} ({Iri.ToString()??Prefix+\":\"+Term,nq})")]
    public class FluentEntityMappingProvider : FluentTermMappingProvider, IEntityMappingProvider
    {
        internal FluentEntityMappingProvider(Type entityType, Iri iri = null, Iri graph = null)
            : base(entityType, iri, graph)
        {
        }

        internal FluentEntityMappingProvider(Type entityType, string prefix = null, string term = null, Iri graph = null)
            : base(entityType, prefix, term, graph)
        {
        }

        internal FluentEntityMappingProvider(Type entityType, string prefix = null, string term = null, string graphPrefix = null, string graphTerm = null)
            : base(entityType, prefix, term, graphPrefix, graphTerm)
        {
        }

        /// <inheritdoc />
        public override void Accept(IMappingProviderVisitor visitor)
        {
            if (visitor == null)
            {
                throw new ArgumentNullException(nameof(visitor));
            }

            visitor.Visit(this);
        }
    }
}
