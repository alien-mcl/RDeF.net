using System;
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

        /// <summary>Allows to register a custom component.</summary>
        /// <typeparam name="TService">Type of the service to register.</typeparam>
        /// <typeparam name="TComponent">Type of the implementation of the <typeparamref name="TService" />.</typeparam>
        /// <param name="lifestyle">Optional lifestyle of the component.</param>
        /// <param name="onActivate">Optional handler invoked after the instance was created.</param>
        /// <returns>Current instance of the <see cref="IComponentConfigurator" />.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "This is a part of the fluent-like API and it is as intended.")]
        IComponentConfigurator WithComponent<TService, TComponent>(Lifestyle lifestyle = Lifestyle.Singleton, Action<IComponentScope, TComponent> onActivate = null)
            where TComponent : TService;

        /// <summary>Allows to register a custom component.</summary>
        /// <typeparam name="TService">Type of the service to register.</typeparam>
        /// <typeparam name="TComponent">Type of the implementation of the <typeparamref name="TService" />.</typeparam>
        /// <param name="name">Optional name of the component.</param>
        /// <param name="lifestyle">Optional lifestyle of the component.</param>
        /// <param name="onActivate">Optional handler invoked after the instance was created.</param>
        /// <returns>Current instance of the <see cref="IComponentConfigurator" />.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "This is a part of the fluent-like API and it is as intended.")]
        IComponentConfigurator WithComponent<TService, TComponent>(string name, Lifestyle lifestyle = Lifestyle.Singleton, Action<IComponentScope, TComponent> onActivate = null)
            where TComponent : TService;

        /// <summary>Allows to register an instance as a <typeparamref name="TService" />.</summary>
        /// <typeparam name="TService">Type of the service being registered.</typeparam>
        /// <param name="instance">Instance to be registered.</param>
        /// <param name="name">Optional name of the instance.</param>
        /// <returns>Current instance of the <see cref="IComponentConfigurator" />.</returns>
        IComponentConfigurator WithInstance<TService>(TService instance, string name = null);
    }
}