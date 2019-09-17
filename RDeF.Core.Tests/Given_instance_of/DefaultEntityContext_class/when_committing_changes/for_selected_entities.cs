using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using RDeF.Vocabularies;

namespace Given_instance_of.DefaultEntityContext_class.when_committing_changes
{
    [TestFixture]
    public class for_selected_entities : ScenarioTest
    {
        public override Task TheTest()
        {
            return Context.Commit(new[] { rdfs.Datatype });
        }

        [Test]
        public void Should_not_detect_changes_in_the_primary_entity()
        {
            ChangeDetector.Verify(instance => instance.Process(PrimaryEntity, RetractedStatements, AddedStatements), Times.Never);
        }

        [Test]
        public void Should_detect_changes_in_the_secondary_entity()
        {
            ChangeDetector.Verify(instance => instance.Process(SecondaryEntity, RetractedStatements, AddedStatements), Times.Once);
        }
    }
}
