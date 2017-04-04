using System;

namespace RDeF.ComponentModel
{
    internal class ComponentRegistration<TService> : IComponentRegistration
    {
        internal ComponentRegistration(Type implementationType)
        {
            ServiceType = typeof(TService);
            ImplementationType = implementationType;
            Instance = null;
        }

        internal ComponentRegistration(TService instance)
        {
            ServiceType = typeof(TService);
            ImplementationType = instance.GetType();
            Instance = instance;
        }

        public Type ServiceType { get; }

        public Type ImplementationType { get; }

        public object Instance { get; }

        public Action<IContainer, object> OnActivate { get; set; }
    }
}
