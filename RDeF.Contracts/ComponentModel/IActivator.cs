using System;

namespace RDeF.ComponentModel
{
    /// <summary>Describes an abstract instance activator.</summary>
    public interface IActivator
    {
        /// <summary>Creates the instance of type <paramref name="type" />.</summary>
        /// <param name="type">Type of the instance to create.</param>
        /// <returns>Instance of <paramref name="type" />.</returns>
        object CreateInstance(Type type);
    }
}
