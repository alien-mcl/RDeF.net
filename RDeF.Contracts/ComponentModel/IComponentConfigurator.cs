using System.Diagnostics.CodeAnalysis;
using RDeF.Mapping;
using RDeF.Mapping.Visitors;

namespace RDeF.ComponentModel
{
    /// <summary>Describes an abstract component configurator.</summary>
    public interface IComponentConfigurator
    {
        /// <summary>Allows to register a custom converter.</summary>
        /// <typeparam name="TConverter">Type of the converter to register.</typeparam>
        /// <returns>Current instance of the <see cref="IComponentConfigurator" />.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "This is a part of the fluent-like API and it is as intended.")]
        IComponentConfigurator WithConverter<TConverter>() where TConverter : ILiteralConverter;

        /// <summary>Allows to register a custom mapping source provider.</summary>
        /// <typeparam name="TMappingsSourceProvider">Type of the mapping source provider.</typeparam>
        /// <returns>Current instance of the <see cref="IComponentConfigurator" />.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "This is a part of the fluent-like API and it is as intended.")]
        IComponentConfigurator WithMappingsProvidedBy<TMappingsSourceProvider>() where TMappingsSourceProvider : IMappingSourceProvider;

        /// <summary>Allows to register a custom mapping provider visitor.</summary>
        /// <typeparam name="TMappingProviderVisitor">Type of the mapping provider visitor.</typeparam>
        /// <returns>Current instance of the <see cref="IComponentConfigurator" />.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "This is a part of the fluent-like API and it is as intended.")]
        IComponentConfigurator WithMappingsProviderVisitor<TMappingProviderVisitor>() where TMappingProviderVisitor : IMappingProviderVisitor;
    }
}