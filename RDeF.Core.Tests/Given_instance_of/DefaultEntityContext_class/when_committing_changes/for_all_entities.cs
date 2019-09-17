using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

namespace Given_instance_of.DefaultEntityContext_class.when_committing_changes
{
    [TestFixture]
    public class for_all_entities : ScenarioTest
    {
        public override Task TheTest()
        {
            return Context.Commit();
        }

        [Test]
        public void Should_detect_changes_in_the_primary_entity()
        {
            ChangeDetector.Verify(instance => instance.Process(PrimaryEntity, RetractedStatements, AddedStatements), Times.Once);
        }

        [Test]
        public void Should_detect_changes_in_the_secondary_entity()
        {
            ChangeDetector.Verify(instance => instance.Process(SecondaryEntity, RetractedStatements, AddedStatements), Times.Once);
        }
    }
}
