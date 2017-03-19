using System;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Mapping.Providers;

namespace Given_instance_of.AttributeEntityMappingProvider_class
{
    public abstract class AttributePropertyMappingProviderTest
    {
        protected static readonly Type EntityType = typeof(IProduct);

        protected AttributeEntityMappingProvider Provider { get; set; }

        public virtual void TheTest()
        {
        }

        [SetUp]
        public void Setup()
        {
            ScenarioSetup();
            TheTest();
        }

        protected virtual void ScenarioSetup()
        {
        }
    }
}
