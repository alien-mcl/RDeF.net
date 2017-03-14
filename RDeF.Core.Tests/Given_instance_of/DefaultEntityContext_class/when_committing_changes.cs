using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Entities;
using RollerCaster;

namespace Given_instance_of.DefaultEntityContext_class
{
    [TestFixture]
    public class when_committing_changes : DefaultEntityContextTest
    {
        private IDictionary<IEntity, ISet<Statement>> RetractedStatements { get; set; }

        private IDictionary<IEntity, ISet<Statement>> AddedStatements { get; set; }

        private Entity Entity { get; set; }

        public override void TheTest()
        {
            Context.Commit();
        }

        [Test]
        public void Should_detect_changes_in_the_entity()
        {
            ChangeDetector.Verify(instance => instance.Process(Entity, RetractedStatements, AddedStatements), Times.Once);
        }

        [Test]
        public void Should_cancel_name_change()
        {
            EntitySource.Verify(instance => instance.Commit(RetractedStatements, AddedStatements), Times.Once);
        }

        protected override void ScenarioSetup()
        {
            Entity = (Entity)Context.Create<IProduct>(new Iri("Test")).Unwrap();
            ChangeDetector.Setup(instance => instance.Process(It.IsAny<Entity>(), It.IsAny<IDictionary<IEntity, ISet<Statement>>>(), It.IsAny<IDictionary<IEntity, ISet<Statement>>>()))
                .Callback<Entity, IDictionary<IEntity, ISet<Statement>>, IDictionary<IEntity, ISet<Statement>>>((entity, retracted, added) =>
                {
                    RetractedStatements = retracted;
                    AddedStatements = added;
                });
            EntitySource.Setup(instance => instance.Commit(It.IsAny<IDictionary<IEntity, ISet<Statement>>>(), It.IsAny<IDictionary<IEntity, ISet<Statement>>>()));
        }
    }
}
