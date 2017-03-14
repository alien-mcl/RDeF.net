using System;
using System.Reflection;

namespace RDeF.ComponentModel
{
    /// <summary>Provides a default <see cref="Activator" /> based implementation of the <see cref="IActivator" />.</summary>
    public class DefaultActivator : IActivator
    {
        /// <inheritdoc />
        public object CreateInstance(Type type)
        {
            return (type.GetTypeInfo().GetConstructor(Type.EmptyTypes) != null ? Activator.CreateInstance(type) :
                type.GetTypeInfo().GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null, Type.EmptyTypes, null));
        }
    }
}
