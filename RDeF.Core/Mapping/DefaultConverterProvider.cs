using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RDeF.Mapping
{
    /// <summary>Provides a default implementation of the <see cref="IConverterProvider" />.</summary>
    public class DefaultConverterProvider : IConverterProvider
    {
        private readonly IEnumerable<ILiteralConverter> _converters;
        private readonly ILiteralConverter _fallbackConverter;

        /// <summary>Initializes a new instance of the <see cref="DefaultConverterProvider" /> class.</summary>
        /// <param name="converters">System registered converter instances.</param>
        public DefaultConverterProvider(IEnumerable<ILiteralConverter> converters)
        {
            if (converters == null)
            {
                throw new ArgumentNullException(nameof(converters));
            }

            if (!converters.Any())
            {
                throw new ArgumentOutOfRangeException(nameof(converters));
            }

            _fallbackConverter = (_converters = converters).First(converter => !converter.SupportedDataTypes.Any() && (!converter.SupportedTypes.Any()));
        }

        /// <inheritdoc />
        public ILiteralConverter FindConverter(Type converterType)
        {
            return (from converter in _converters
                    let match = converterType == converter.GetType() ? 2 : (converterType.GetTypeInfo().IsInstanceOfType(converter) ? 1 : 0)
                    where match > 0
                    orderby match descending
                    select converter).First();
        }

        /// <inheritdoc />
        public ILiteralConverter FindLiteralConverter(Type valueType)
        {
            return (from converter in _converters
                    from supportedType in converter.SupportedTypes
                    where supportedType == valueType
                    select converter).SingleOrDefault() ?? _fallbackConverter;
        }
    }
}
