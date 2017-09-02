using System;
using System.Diagnostics.CodeAnalysis;
using RDeF.Entities;
using RDeF.Mapping.Providers;
using RollerCaster.Reflection;

namespace RDeF.Mapping.Visitors
{
    /// <summary>Applies a literal type converter based on how the mapped property is defined.</summary>
    public class ConverterConventionVisitor : IMappingProviderVisitor
    {
        private readonly IConverterProvider _converterProvider;

        /// <summary>Initializes a new instance of the <see cref="ConverterConventionVisitor" /> class.</summary>
        /// <param name="converterProvider">Converters provider.</param>
        public ConverterConventionVisitor(IConverterProvider converterProvider)
        {
            if (converterProvider == null)
            {
                throw new ArgumentNullException(nameof(converterProvider));
            }

            _converterProvider = converterProvider;
        }

        /// <inheritdoc />
        public void Visit(ICollectionMappingProvider collectionMappingProvider)
        {
            Visit((IPropertyMappingProvider)collectionMappingProvider);
        }

        /// <inheritdoc />
        public void Visit(IPropertyMappingProvider propertyMappingProvider)
        {
            if (propertyMappingProvider == null)
            {
                throw new ArgumentNullException(nameof(propertyMappingProvider));
            }

            if (propertyMappingProvider.ValueConverterType != null)
            {
                return;
            }

            var valueType = propertyMappingProvider.Property.PropertyType.GetItemType();
            if (typeof(IEntity).IsAssignableFrom(valueType))
            {
                return;
            }

            propertyMappingProvider.ValueConverterType = _converterProvider.FindLiteralConverter(valueType).GetType();
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "Method is not used.")]
        public void Visit(IDictionaryMappingProvider dictionaryMappingProvider)
        {
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "Method is not used.")]
        public void Visit(IEntityMappingProvider entityMappingProvider)
        {
        }
    }
}
