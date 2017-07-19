using System;

namespace RDeF.ComponentModel
{
    internal class ComponentRegistration<TService> : IComponentRegistration
    {
        private readonly int _hashCode;

        internal ComponentRegistration(Type implementationType, string name)
        {
            ServiceType = typeof(TService);
            ImplementationType = implementationType;
            Instance = null;
            Name = name;
            _hashCode = 13 * (name?.GetHashCode() ?? ServiceType.GetHashCode());
        }

        internal ComponentRegistration(TService instance, string name)
        {
            ServiceType = typeof(TService);
            ImplementationType = instance.GetType();
            Instance = instance;
            Name = name;
            _hashCode = 17 * (name?.GetHashCode() ?? ServiceType.GetHashCode());
        }

        public Type ServiceType { get; }

        public string Name { get; }

        public Type ImplementationType { get; }

        public object Instance { get; }

        public Action<IContainer, object> OnActivate { get; set; }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return _hashCode;
        }

        public override bool Equals(object obj)
        {
            var registration = obj as IComponentRegistration;
            if ((registration == null) || (registration.GetType() != GetType()))
            {
                return false;
            }

            return ServiceType == registration.ServiceType && Name == registration.Name;
        }
    }
}
