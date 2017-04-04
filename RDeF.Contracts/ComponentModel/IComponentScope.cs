using System;
using System.Diagnostics.CodeAnalysis;

namespace RDeF.ComponentModel
{
    /// <summary>Describes an abstract components scope.</summary>
    public interface IComponentScope
    {
        /// <summary>Checks wheter any type assignable to <typeparamref name="TService" /> is registered.</summary>
        /// <typeparam name="TService">Type of the service to check.</typeparam>
        /// <returns><b>true</b> if there is at least one registration for the <typeparamref name="TService" />; otherwise <b>false</b>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "This is a part of a fluent-like API and works as intended.")]
        bool IsRegistered<TService>();

        /// <summary>Resolves an instance of assignable to <typeparamref name="TService" />.</summary>
        /// <typeparam name="TService">Type of the service to resolve.</typeparam>
        /// <returns>Instance assignable to <typeparamref name="TService" /> if registered; otherwise <b>null</b>.</returns>
        TService Resolve<TService>();

        /// <summary>Resolves an instance of assignable to <typeref name="type" />.</summary>
        /// <param name="type">Type of the service to resolve.</param>
        /// <returns>Instance assignable to <typeref name="type" /> if registered; otherwise <b>null</b>.</returns>
        object Resolve(Type type);
    }
}
