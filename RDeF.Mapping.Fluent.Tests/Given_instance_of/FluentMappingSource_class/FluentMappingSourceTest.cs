using System.Reflection;
using NUnit.Framework;
using RDeF.Entities;
using RDeF.Mapping;

namespace Given_instance_of.FluentMappingSource_class
{
    public abstract class FluentMappingSourceTest
    {
        protected static readonly QIriMapping QIriMapping = new QIriMapping("prefix", new Iri("test:"));
        
        protected FluentMappingSource Source { get; private set; }

        public virtual void TheTest()
        {
        }

        [SetUp]
        public void Setup()
        {
            Source = new FluentMappingSource(GetType().GetTypeInfo().Assembly, new[] { QIriMapping });
            ScenarioSetup();
            TheTest();
        }

        protected virtual void ScenarioSetup()
        {
        }
    }
}
