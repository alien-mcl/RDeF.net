using System;
using System.Diagnostics.CodeAnalysis;
using RDeF.Mapping.Converters;

namespace RDeF.ComponentModel
{
    /// <summary>Registers default converters.</summary>
    public class ConvertersModule : IModule
    {
        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [SuppressMessage("TS0000", "NoUnitTests", Justification = "This is an IoC container initialization routine. No need to test it.")]
        public void Initialize(IComponentConfigurator componentConfigurator)
        {
            if (componentConfigurator == null)
            {
                throw new ArgumentNullException(nameof(componentConfigurator));
            }

            componentConfigurator.WithConverter<Base64BinaryConverter>();
            componentConfigurator.WithConverter<BooleanConverter>();
            componentConfigurator.WithConverter<DateTimeConverter>();
            componentConfigurator.WithConverter<DecimalConverter>();
            componentConfigurator.WithConverter<DurationConverter>();
            componentConfigurator.WithConverter<FloatingPointLiteralConverter>();
            componentConfigurator.WithConverter<GuidConverter>();
            componentConfigurator.WithConverter<IntegerConverter>();
            componentConfigurator.WithConverter<StringConverter>();
            componentConfigurator.WithConverter<UntypedLiteralConverter>();
            componentConfigurator.WithConverter<UriConverter>();
            componentConfigurator.WithConverter<IriConverter>();
        }
    }
}
