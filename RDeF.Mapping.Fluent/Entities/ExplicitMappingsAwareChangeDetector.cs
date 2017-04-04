using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using RDeF.Entities;
using RDeF.Mapping.Explicit;

namespace RDeF.Mapping.Entities
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Instance is instatiated by an IoC container.")]
    internal class ExplicitMappingsAwareChangeDetector : DefaultChangeDetector
    {
        private readonly IExplicitMappings _entityMappings;

        /// <summary>Initializes a new instance of the <see cref="ExplicitMappingsAwareChangeDetector" /> class.</summary>
        /// <param name="mappingsRepository">Mappings repository.</param>
        /// <param name="entityMappings">Explicit entity mappings.</param>
        internal ExplicitMappingsAwareChangeDetector(IMappingsRepository mappingsRepository, IExplicitMappings entityMappings)
            : base(mappingsRepository)
        {
            _entityMappings = entityMappings;
        }

        /// <inheritdoc />
        protected override IEntityMapping GetEntityMapping(Type type)
        {
            return _entityMappings.FindEntityMappingFor(type) ?? base.GetEntityMapping(type);
        }

        protected override IPropertyMapping GetPropertyMapping(PropertyInfo property)
        {
            return _entityMappings.FindPropertyMappingFor(property) ?? base.GetPropertyMapping(property);
        }
    }
}
