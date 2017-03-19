using System;
using System.Reflection;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Mapping.Providers;

namespace Given_instance_of.AttributeCollectionMappingProvider_class
{
    public abstract class AttributeCollectionMappingProviderTest
    {
        protected static readonly Type EntityType = typeof(IProduct);
        protected static readonly PropertyInfo Property = EntityType.GetTypeInfo().GetProperty("Categories");

        protected AttributeCollectionMappingProvider Provider { get; set; }

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
