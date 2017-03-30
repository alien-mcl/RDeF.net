using System;
using System.Reflection;
using RDeF.Mapping.Visitors;

namespace RDeF.Mapping.Providers
{
    internal class ClosedGenericPropertyMappingProvider : ClosedGenericTermMappingProvider, IPropertyMappingProvider
    {
        private readonly IPropertyMappingProvider _openGenericPropertyMappingProvider;

        internal ClosedGenericPropertyMappingProvider(Type closedGenericType, IPropertyMappingProvider openGenericPropertyMappingProvider)
            : base(closedGenericType, openGenericPropertyMappingProvider)
        {
            _openGenericPropertyMappingProvider = openGenericPropertyMappingProvider;
        }

        /// <inheritdoc />
        public PropertyInfo Property { get { return _openGenericPropertyMappingProvider.Property; } }

        /// <inheritdoc />
        public Type ValueConverterType
        {
            get { return _openGenericPropertyMappingProvider.ValueConverterType; }
            set { _openGenericPropertyMappingProvider.ValueConverterType = value; }
        }

        /// <inheritdoc />
        public override void Accept(IMappingProviderVisitor visitor)
        {
            visitor?.Visit(this);
        }
    }
}
