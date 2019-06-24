using NUnit.Framework;
using RDeF.Entities;
using RDeF.Mapping;

namespace Given_instance_of.InternalMappingSource_class
{
    public abstract class InternalMappingSourceTest
    {
        protected InternalMappingSource Source { get; private set; }

        public virtual void TheTest()
        {
        }

        [SetUp]
        public void Setup()
        {
            Source = new InternalMappingSource(typeof(ITypedEntity).Assembly);
            ScenarioSetup();
            TheTest();
        }

        protected virtual void ScenarioSetup()
        {
        }
    }
}
