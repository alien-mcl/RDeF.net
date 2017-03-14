using FluentAssertions;
using Moq;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Entities;
using RDeF.Mapping;
using RollerCaster;

namespace Given_instance_of.SimpleInMemoryEntitySource_class
{
    [TestFixture]
    public class when_creating_an_entity : SimpleInMemoryEntitySourceTest
    {
        private DefaultEntityContext Context { get; set; }

        private IProduct Result { get; set; }

        public override void TheTest()
        {
            Result = EntitySource.Create<IProduct>(new Iri("test"), Context);
        }

        [Test]
        public void Should_create_new_entity_already_initialized()
        {
            ((Entity)Result.Unwrap()).IsInitialized.Should().BeTrue();
        }

        protected override void ScenarioSetup()
        {
            Context = new DefaultEntityContext(EntitySource, new Mock<IMappingsRepository>(MockBehavior.Strict).Object, new Mock<IChangeDetector>(MockBehavior.Strict).Object);
        }
    }
}
