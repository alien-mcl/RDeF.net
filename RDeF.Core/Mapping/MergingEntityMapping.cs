﻿using System;
using System.Collections.Generic;
using RDeF.Entities;

namespace RDeF.Mapping
{
    internal sealed class MergingEntityMapping : IEntityMapping
    {
        internal MergingEntityMapping(Type type)
        {
            Type = type;
            Classes = new HashSet<Iri>();
            Properties = new HashSet<IPropertyMapping>(PropertyMappingEqualityComparer.Default);
        }

        /// <inheritdoc />
        public Type Type { get; }

        /// <summary>Gets the list of classes this entity mapping maps.</summary>
        public ISet<Iri> Classes { get; }

        /// <summary>Gets the property mappings.</summary>
        public ISet<IPropertyMapping> Properties { get; }

        /// <inheritdoc />
        IEnumerable<Iri> IEntityMapping.Classes { get { return Classes; } }

        /// <inheritdoc />
        IEnumerable<IPropertyMapping> IEntityMapping.Properties { get { return Properties; } }
    }
}
