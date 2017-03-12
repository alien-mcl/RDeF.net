using System;
using System.Reflection;

namespace RDeF.Mapping
{
    /// <summary>Provides a default implementation of the <see cref="IMappingsBuilder" />.</summary>
    /// <seealso cref="RDeF.Mapping.IMappingsBuilder" />
    public sealed class DefaultMappingsBuilder : IMappingsBuilder
    {
        private readonly Action<Assembly> _assemblyRegistrationDelegate;

        internal DefaultMappingsBuilder(Action<Assembly> assemblyRegistrationDelegate)
        {
            _assemblyRegistrationDelegate = assemblyRegistrationDelegate;
        }

        /// <inheritdoc />
        public IMappingsBuilder FromAssemblyOf<T>()
        {
            _assemblyRegistrationDelegate(typeof(T).GetTypeInfo().Assembly);
            return this;
        }
    }
}
