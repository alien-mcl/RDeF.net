using System;
using RDeF.ComponentModel;
using RDeF.Mapping;

namespace RDeF.Entities
{
    /// <summary>Represents an <see cref="IEntityContext" /> factory.</summary>
    public interface IEntityContextFactory
    {
        /// <summary>Creates a new instance of the <see cref="IEntityContext" />.</summary>
        /// <returns>Newly created instance of the <see cref="IEntityContext" />.</returns>
        IEntityContext Create();

        /// <summary>Defines a type of the activator to be used.</summary>
        /// <remarks>Factory assumes that the type has a default parameterless constructor and it will configure itself by its own.</remarks>
        /// <typeparam name="T">Type of the activator to use.</typeparam>
        /// <returns>Current instance of the <see cref="IEntityContextFactory" />.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "This is a part of a fluent-like API.")]
        IEntityContextFactory WithActivator<T>();

        /// <summary>Defines an activator to be used.</summary>
        /// <param name="activator">Target activator to be used.</param>
        /// <returns>Current instance of the <see cref="IEntityContextFactory" />.</returns>
        IEntityContextFactory WithActivator(IActivator activator);

        /// <summary>Defines a type of the entity source to be used.</summary>
        /// <remarks>Factory assumes that the type has a default parameterless constructor and it will configure itself by its own.</remarks>
        /// <typeparam name="T">Type of the entity source to use.</typeparam>
        /// <returns>Current instance of the <see cref="IEntityContextFactory" />.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "This is a part of a fluent-like API.")]
        IEntityContextFactory WithEntitySource<T>();

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
