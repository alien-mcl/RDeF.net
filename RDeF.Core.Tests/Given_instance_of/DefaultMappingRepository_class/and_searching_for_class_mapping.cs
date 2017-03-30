using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Entities;
using RDeF.Mapping;
using RDeF.Mapping.Providers;

namespace Given_instance_of.DefaultMappingRepository_class.which_is_already_initialized
{
    [TestFixture]
    public class and_searching_for_class_mapping : DefaultMappingRepositoryTest
    {
        private const string ExpectedClass = "Product";

        private IEntityMapping Result { get; set; }

        public override void TheTest()
        {
            Result = MappingRepository.FindEntityMappingFor(new Iri(ExpectedClass));
        }

        [Test]
        public void Should_retrieve_the_entity_mapping()
        {
            Result.Classes.Where(@class => @class.Term == new Iri(ExpectedClass)).Should().HaveCount(1);
        }

        [Test]
        public void Should_throw_when_no_class_is_given()
        {
            MappingRepository.Invoking(instance => instance.FindEntityMappingFor((Iri)null)).ShouldThrow<ArgumentNullException>();
        }

        protected override void ScenarioSetup()
        {
            var entityMapping = new Mock<IEntityMapping>(MockBehavior.Strict);
            entityMapping.SetupGet(instance => instance.Type).Returns(typeof(IProduct));
            var classMapping = new Mock<IStatementMapping>(MockBehavior.Strict);
            classMapping.SetupGet(instance => instance.Term).Returns(new Iri(ExpectedClass));
            classMapping.SetupGet(instance => instance.Graph).Returns((Iri)null);
            entityMapping.SetupGet(instance => instance.Classes).Returns(new[] { classMapping.Object });
            MappingBuilder.Setup(instance => instance.BuildMappings(It.IsAny<IEnumerable<IMappingSource>>(), It.IsAny<IDictionary<Type, ICollection<ITermMappingProvider>>>()))
                .Returns(new Dictionary<Type, IEntityMapping>() { { typeof(IProduct), entityMapping.Object } });
        }
    }
}
