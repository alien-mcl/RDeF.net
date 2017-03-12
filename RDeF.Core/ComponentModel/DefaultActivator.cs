using System;

namespace RDeF.ComponentModel
{
    /// <summary>Provides a default <see cref="Activator" /> based implementation of the <see cref="IActivator" />.</summary>
    public class DefaultActivator : IActivator
    {
        /// <inheritdoc />
        public object CreateInstance(Type type)
        {
            return Activator.CreateInstance(type);
        }
    }
}
