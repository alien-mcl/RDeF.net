using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Entities;
using RDeF.Mapping;
using RollerCaster;

namespace Given_instance_of.Entity_class
{
    [TestFixture]
    public class when_verifying_mapped_classes : EntityTest
    {
        private static readonly Iri Class1 = new Iri("class1");
        private static readonly Iri Class2 = new Iri("class2");

        private Mock<IMappingsRepository> Mappings { get; set; }

        private Mock<IPropertyMapping> PropertyMapping { get; set; }

        private Mock<IEntityMapping> EntityMapping { get; set; }

        private Mock<IStatementMapping> PrimaryClassMapping { get; set; }

        private Mock<IStatementMapping> SecondaryClassMapping { get; set; }

        [Test]
        public void Should_discover_matching_classes_correctly()
        {
            Entity.Is(Class1, Class2).Should().BeTrue();
        }

        [Test]
        public void Should_discover_matching_class_correctly()
        {
            Entity.Is(Class1).Should().BeTrue();
        }

        protected override void ScenarioSetup()
        {
            PropertyMapping = new Mock<IPropertyMapping>();
            PrimaryClassMapping = new Mock<IStatementMapping>(MockBehavior.Strict);
            PrimaryClassMapping.SetupGet(instance => instance.Term).Returns(Class1);
            SecondaryClassMapping = new Mock<IStatementMapping>(MockBehavior.Strict);
            SecondaryClassMapping.SetupGet(instance => instance.Term).Returns(Class2);
            EntityMapping = new Mock<IEntityMapping>(MockBehavior.Strict);
            EntityMapping.SetupGet(instance => instance.Classes).Returns(new[] { PrimaryClassMapping.Object, SecondaryClassMapping.Object });
            Mappings = new Mock<IMappingsRepository>(MockBehavior.Strict);
            Mappings.Setup(instance => instance.FindEntityMappingFor(It.IsAny<IEntity>(), It.IsAny<Type>())).Returns(EntityMapping.Object);
            Mappings.Setup(instance => instance.FindPropertyMappingFor(It.IsAny<IEntity>(), It.IsAny<PropertyInfo>()))
                .Returns(PropertyMapping.Object);
            Context.Setup(instance => instance.Initialize(It.IsAny<Entity>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            Context.SetupGet(instance => instance.Mappings).Returns(Mappings.Object);
            Entity.ActLike<IProduct>();
        }
    }
}
