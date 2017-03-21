using System;
using System.Collections.Generic;
using System.Linq;
using RDeF.Entities;
using RDeF.Mapping.Providers;
using RollerCaster.Reflection;

namespace RDeF.Mapping.Visitors
{
    /// <summary>Applies a literal type converter based on how the mapped property is defined.</summary>
    public class ConverterConventionVisitor : IMappingProviderVisitor
    {
        private readonly IEnumerable<ILiteralConverter> _converters;

        /// <summary>Initializes a new instance of the <see cref="ConverterConventionVisitor" /> class.</summary>
        /// <param name="converters">Registered value converters.</param>
        public ConverterConventionVisitor(IEnumerable<ILiteralConverter> converters)
        {
            if (converters == null)
            {
                throw new ArgumentNullException(nameof(converters));
            }

            _converters = converters;
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

            propertyMappingProvider.ValueConverterType = (from converter in _converters
                                                          from supportedType in converter.SupportedTypes
                                                          where supportedType == valueType
                                                          select converter).Single().GetType();
        }

        /// <inheritdoc />
        public void Visit(IDictionaryMappingProvider dictionaryMappingProvider)
        {
        }

        /// <inheritdoc />
        public void Visit(IEntityMappingProvider entityMappingProvider)
        {
        }
    }
}
