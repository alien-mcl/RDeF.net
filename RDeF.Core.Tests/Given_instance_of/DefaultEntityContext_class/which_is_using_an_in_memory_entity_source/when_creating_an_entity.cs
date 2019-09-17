using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Entities;

namespace Given_instance_of.DefaultEntityContext_class.which_is_using_an_in_memory_entity_source
{
    [TestFixture]
    public class when_creating_an_entity : ScenarioTest
    {
        private static readonly Iri Iri = new Iri("test");

        public override Task TheTest()
        {
            Context.Create<IProduct>(Iri);
            return Task.CompletedTask;
        }

        [Test]
        public void Should_let_the_entity_source_to_create_an_entity()
        {
            InMemoryEntitySource.Verify(instance => instance.Create(Iri), Times.Once);
        }

        protected override void ScenarioSetup()
        {
            base.ScenarioSetup();
            InMemoryEntitySource.Setup(instance => instance.Create(It.IsAny<Iri>()))
                .Returns<Iri>(iri => new Entity(iri, Context));
        }
    }
}
