using System;
using System.Collections.Generic;

namespace RDeF.Mapping
{
    internal sealed class MergingEntityMapping : IEntityMapping
    {
        internal MergingEntityMapping(Type type)
        {
            Type = type;
            Classes = new HashSet<IStatementMapping>(StatementMappingEqualityComparer.Default);
            Properties = new HashSet<IPropertyMapping>(PropertyMappingEqualityComparer.Default);
        }

        /// <inheritdoc />
        public Type Type { get; }

        /// <summary>Gets the list of classes this entity mapping maps.</summary>
        public ISet<IStatementMapping> Classes { get; }

        /// <summary>Gets the property mappings.</summary>
        public ISet<IPropertyMapping> Properties { get; }

        /// <inheritdoc />
        IEnumerable<IStatementMapping> IEntityMapping.Classes { get { return Classes; } }

        /// <inheritdoc />
        IEnumerable<IPropertyMapping> IEntityMapping.Properties { get { return Properties; } }
    }
}
