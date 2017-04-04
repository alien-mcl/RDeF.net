using Moq;
using RDeF.Entities;

namespace Given_instance_of.DefaultEntityContext_class.which_is_using_an_in_memory_entity_source
{
    public abstract class ScenarioTest : DefaultEntityContextTest
    {
        internal Mock<IInMemoryEntitySource> InMemoryEntitySource { get; private set; }

        protected override void ScenarioSetup()
        {
            EntitySource = (InMemoryEntitySource = new Mock<IInMemoryEntitySource>(MockBehavior.Strict)).As<IEntitySource>();
            Context = new DefaultEntityContext(EntitySource.Object, MappingsRepository.Object, ChangeDetector.Object);
        }
    }
}
