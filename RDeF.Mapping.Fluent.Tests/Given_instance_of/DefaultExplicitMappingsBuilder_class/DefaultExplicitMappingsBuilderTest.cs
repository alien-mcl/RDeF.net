using NUnit.Framework;
using RDeF.Data;
using RDeF.Mapping.Explicit;

namespace Given_instance_of.DefaultExplicitMappingsBuilder_class
{
    public abstract class DefaultExplicitMappingsBuilderTest
    {
        protected DefaultExplicitMappingsBuilder<IProduct> Builder { get; private set; }

        public virtual void TheTest()
        {
        }

        [SetUp]
        public void Setup()
        {
            Builder = new DefaultExplicitMappingsBuilder<IProduct>();
            ScenarioSetup();
            TheTest();
        }

        protected virtual void ScenarioSetup()
        {
        }
    }
}
