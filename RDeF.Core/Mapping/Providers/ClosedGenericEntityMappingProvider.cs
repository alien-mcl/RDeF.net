using System;
using RDeF.Mapping.Visitors;

namespace RDeF.Mapping.Providers
{
    internal class ClosedGenericEntityMappingProvider : ClosedGenericTermMappingProvider, IEntityMappingProvider
    {
        internal ClosedGenericEntityMappingProvider(Type closedGenericType, IEntityMappingProvider openGenericEntityMappingProvider)
            : base(closedGenericType, openGenericEntityMappingProvider)
        {
        }

        /// <inheritdoc />
        public override void Accept(IMappingProviderVisitor visitor)
        {
            visitor?.Visit(this);
        }
    }
}
