using System.Linq;
using Moq;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Entities;
using RDeF.Mapping;

namespace Given_instance_of.DefaultMappingRepository_class
{
    public abstract class DefaultMappingRepositoryTest
    {
        protected DefaultMappingRepository MappingRepository { get; set; }

        protected Mock<IMappingSource> MappingSource { get; private set; }

        public virtual void TheTest()
        {
        }

        [SetUp]
        public void Setup()
        {
            MappingSource = new Mock<IMappingSource>(MockBehavior.Strict);
            ScenarioSetup();
            TheTest();
        }

        protected static Mock<IEntityMapping> SetupEntityMapping(IConverter converter, string @class, params string[] properties)
        {
            var result = new Mock<IEntityMapping>(MockBehavior.Strict);
            result.SetupGet(instance => instance.Type).Returns(typeof(IProduct));
            result.SetupGet(instance => instance.Classes).Returns(new[] { new Iri(@class) });
            result.SetupGet(instance => instance.Properties)
                .Returns(properties.Select(property => SetupPropertyMapping(property, converter).Object));
            return result;
        }

        protected static Mock<IPropertyMapping> SetupPropertyMapping(string name, IConverter converter)
        {
            var result = new Mock<IPropertyMapping>(MockBehavior.Strict);
            result.SetupGet(instance => instance.Predicate).Returns(new Iri(name));
            result.SetupGet(instance => instance.Name).Returns(name);
            result.SetupGet(instance => instance.Graph).Returns((Iri)null);
            result.SetupGet(instance => instance.ValueConverter).Returns(converter);
            return result;
        }

        protected virtual void ScenarioSetup()
        {
        }
    }
}
