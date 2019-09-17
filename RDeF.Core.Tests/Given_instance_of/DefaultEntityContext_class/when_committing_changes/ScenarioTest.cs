using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Entities;
using RDeF.Mapping;
using RDeF.Vocabularies;
using RollerCaster;

namespace Given_instance_of.DefaultEntityContext_class.when_committing_changes
{
    public abstract class ScenarioTest : DefaultEntityContextTest
    {
        internal Entity PrimaryEntity { get; private set; }

        internal Entity SecondaryEntity { get; private set; }

        protected ICollection<Iri> DeletedEntities { get; private set; }

        protected IDictionary<IEntity, ISet<Statement>> RetractedStatements { get; private set; }

        protected IDictionary<IEntity, ISet<Statement>> AddedStatements { get; private set; }
        
        [Test]
        public void Should_pass_all_changes_to_the_underlying_EntitySource()
        {
            EntitySource.Verify(
                instance => instance.Commit(DeletedEntities, RetractedStatements, AddedStatements, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        protected override void ScenarioSetup()
        {
            PrimaryEntity = (Entity)Context.Create<IProduct>(rdfs.Class).Unwrap();
            SecondaryEntity = (Entity)Context.Create<IProduct>(rdfs.Datatype).Unwrap();
            ChangeDetector.Setup(instance => instance.Process(
                    It.IsAny<Entity>(),
                    It.IsAny<IDictionary<IEntity, ISet<Statement>>>(),
                    It.IsAny<IDictionary<IEntity, ISet<Statement>>>()))
                .Callback<Entity, IDictionary<IEntity, ISet<Statement>>, IDictionary<IEntity, ISet<Statement>>>((entity, retracted, added) =>
                {
                    RetractedStatements = retracted;
                    AddedStatements = added;
                });
            DeletedEntities = new List<Iri>();
            EntitySource.Setup(instance => instance.Commit(
                It.IsAny<IEnumerable<Iri>>(),
                It.IsAny<IDictionary<IEntity, ISet<Statement>>>(),
                It.IsAny<IDictionary<IEntity, ISet<Statement>>>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            MappingsRepository.Setup(instance => instance.FindPropertyMappingFor(It.IsAny<IEntity>(), It.IsAny<PropertyInfo>()))
                .Returns<IEntity, PropertyInfo>((entity, propertyInfo) =>
                {
                    var result = new Mock<IPropertyMapping>(MockBehavior.Strict);
                    result.SetupGet(instance => instance.PropertyInfo).Returns(propertyInfo);
                    return result.Object;
                });
        }
    }
}
