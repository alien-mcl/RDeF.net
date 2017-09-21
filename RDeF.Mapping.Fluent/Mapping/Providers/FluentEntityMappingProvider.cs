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
