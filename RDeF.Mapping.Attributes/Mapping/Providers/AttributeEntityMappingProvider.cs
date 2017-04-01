using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using RDeF.Entities;
using RDeF.Mapping.Attributes;
using RDeF.Mapping.Visitors;

namespace RDeF.Mapping.Providers
{
    /// <summary>Represents an attribute based property mapping.</summary>
    [DebuggerDisplay("{EntityType.Name,nq} ({Iri.ToString()??Prefix+\":\"+Term,nq})")]
    public class AttributeEntityMappingProvider : AttributeTermMappingProvider, IEntityMappingProvider
    {
        internal AttributeEntityMappingProvider(Type entityType, Iri iri = null, Iri graph = null)
            : base(entityType, iri, graph)
        {
        }

        internal AttributeEntityMappingProvider(Type entityType, string prefix = null, string term = null, Iri graph = null)
            : base(entityType, prefix, term, graph)
        {
        }

        internal AttributeEntityMappingProvider(Type entityType, string prefix = null, string term = null, string graphPrefix = null, string graphTerm = null)
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

        internal static AttributeEntityMappingProvider FromAttribute(Type type, ClassAttribute classMapping)
        {
            var types = new List<Type>() { typeof(Type) };
            var parameters = new List<object>() { type };
            AddTerm(types, parameters, classMapping.MappedIri, classMapping.Prefix, classMapping.Term);
            AddTerm(types, parameters, classMapping.GraphIri, classMapping.GraphPrefix, classMapping.GraphTerm);
#if NETSTANDARD1_6
            return (AttributeEntityMappingProvider)typeof(AttributeEntityMappingProvider)
                .GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)
                .First(ctor => ctor.GetParameters().Select(parameter => parameter.ParameterType).SequenceEqual(types)).Invoke(parameters.ToArray());
#else
            return (AttributeEntityMappingProvider)typeof(AttributeEntityMappingProvider)
                .GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, types.ToArray(), null).Invoke(parameters.ToArray());
#endif
        }
    }
}
