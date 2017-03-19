using System.Reflection;
using NUnit.Framework;
using RDeF.Mapping;

namespace Given_instance_of.AttributeMappingSource_class
{
    public abstract class AttributeMappingSourceTest
    {
        protected AttributesMappingSource Source { get; private set; }

        public virtual void TheTest()
        {
        }

        [SetUp]
        public void Setup()
        {
            Source = new AttributesMappingSource(GetType().GetTypeInfo().Assembly);
            ScenarioSetup();
            TheTest();
        }

        protected virtual void ScenarioSetup()
        {
        }
    }
}
