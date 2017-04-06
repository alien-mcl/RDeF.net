using System.Reflection;
using NUnit.Framework;
using RDeF.Mapping;

namespace Given_instance_of.FluentMappingSource_class
{
    public abstract class FluentMappingSourceTest
    {
        protected FluentMappingSource Source { get; private set; }

        public virtual void TheTest()
        {
        }

        [SetUp]
        public void Setup()
        {
            Source = new FluentMappingSource(GetType().GetTypeInfo().Assembly);
            ScenarioSetup();
            TheTest();
        }

        protected virtual void ScenarioSetup()
        {
        }
    }
}
