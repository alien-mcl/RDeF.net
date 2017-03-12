using System;
using System.Collections.Generic;
using System.Diagnostics;
using RDeF.Entities;

namespace RDeF.Mapping.Attributes
{
    /// <summary>Provides an attribute based <see cref="IEntityMapping" />.</summary>
    [DebuggerDisplay("{Type.Name,nq} ({System.String.Join(\", \", Classes),nq})")]
    public sealed class AttributeEntityMapping : IEntityMapping
    {
        internal AttributeEntityMapping(Type type)
        {
            Type = type;
            Classes = new HashSet<Iri>();
            Properties = new HashSet<AttributePropertyMapping>();
        }

        /// <summary>Gets the type being mapped.</summary>
        public Type Type { get; }

        /// <summary>Gets the classes this mapping maps for the given <see cref="AttributeEntityMapping.Type" />.</summary>
        public ISet<Iri> Classes { get; }

        /// <summary>Gets the property mappings.</summary>
        public ISet<AttributePropertyMapping> Properties { get; }

        /// <inheritdoc />
        IEnumerable<Iri> IEntityMapping.Classes { get { return Classes; } }

        /// <inheritdoc />
        IEnumerable<IPropertyMapping> IEntityMapping.Properties { get { return Properties; } }
    }
}
