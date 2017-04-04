using System;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RDeF;
using RDeF.Data;
using RDeF.Entities;
using RDeF.Mapping;
using RDeF.Mapping.Converters;
using RDeF.Mapping.Entities;
using RDeF.Mapping.Explicit;
using RDeF.Mapping.Visitors;

namespace Given_a_context.with_explicitly_mapped_entity
{
    [TestFixture]
    public class when_creating_an_entity_with_explicit_mappings : ExplicitMappingsTest
    {
        internal Mock<IExplicitMappings> Mappings { get; set; }

        protected IEntityMapping Mapping { get; set; }

        public override void TheTest()
        {
            Context.Create<IUnmappedProduct>(new Iri("test"), MapEntity);
        }

        [Test]
        public void Should_throw_when_no_entity_context_is_given()
        {
            ((IEntityContext)null).Invoking(_ => _.Create<IUnmappedProduct>(new Iri("test"), null)).ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void Should_create_entity_mappings_accordingly()
        {
            Mapping.Should().BeAssignableTo<IEntityMapping>().ContainMappingsFor(typeof(IUnmappedProduct));
        }

        protected override void ScenarioSetup()
        {
            Mappings = new Mock<IExplicitMappings>(MockBehavior.Strict);
            Mappings.Setup(instance => instance.Set(It.IsAny<IEntityMapping>())).Callback<IEntityMapping>(mapping => Mapping = mapping);
            var context = new Mock<IEntityContext>(MockBehavior.Strict);
            context.Setup(instance => instance.Create<IUnmappedProduct>(It.IsAny<Iri>())).Returns(new Mock<IUnmappedProduct>(MockBehavior.Strict).Object);
            EntityContextExtensions.ExplicitMappings[Context = context.Object] = Mappings.Object;
            EntityContextExtensions.LiteralConverters = new[] { new StringConverter() };
            EntityContextExtensions.MappingVisitors = Array.Empty<IMappingProviderVisitor>();
            EntityContextExtensions.QIriMappings = Array.Empty<QIriMapping>();
        }
    }
}
