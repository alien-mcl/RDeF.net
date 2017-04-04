using System;
using System.Diagnostics.CodeAnalysis;
using RDeF.ComponentModel;
using RDeF.Mapping;

namespace RDeF.Entities
{
    /// <summary>Represents an <see cref="IEntityContext" /> factory.</summary>
    public interface IEntityContextFactory : IDisposable
    {
        /// <summary>Gets the mappings repository.</summary>
        IMappingsRepository Mappings { get; }

        /// <summary>Creates a new instance of the <see cref="IEntityContext" />.</summary>
        /// <returns>Newly created instance of the <see cref="IEntityContext" />.</returns>
        IEntityContext Create();

        /// <summary>Registers a given <typeparamref name="TModule" /> for component configuration.</summary>
        /// <typeparam name="TModule">Type of the module.</typeparam>
        /// <returns>Current instance of the <see cref="IEntityContextFactory" />.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "This is a part of a fluent-like API and it is as intended.")]
        IEntityContextFactory WithModule<TModule>() where TModule : IModule;

        /// <summary>Defines a type of the entity source to be used.</summary>
        /// <remarks>Factory assumes that the type has a default parameterless constructor and it will configure itself by its own.</remarks>
        /// <typeparam name="T">Type of the entity source to use.</typeparam>
        /// <returns>Current instance of the <see cref="IEntityContextFactory" />.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "This is a part of a fluent-like API and it is as intended.")]
        IEntityContextFactory WithEntitySource<T>() where T : IEntitySource;

        /// <summary>Defines an entity source to be used.</summary>
        /// <param name="entitySource">Target entity source to be used.</param>
        /// <returns>Current instance of the <see cref="IEntityContextFactory" />.</returns>
        IEntityContextFactory WithEntitySource(IEntitySource entitySource);

        /// <summary>Allows caller to define mappings to be registered.</summary>
        /// <param name="builder">Mappings builder.</param>
        /// <returns>Current instance of the <see cref="IEntityContextFactory" />.</returns>
        IEntityContextFactory WithMappings(Action<IMappingsBuilder> builder);
    }
}
