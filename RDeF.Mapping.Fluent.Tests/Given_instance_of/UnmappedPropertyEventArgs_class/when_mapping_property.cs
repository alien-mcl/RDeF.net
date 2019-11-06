using System;
using System.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Entities;
using RDeF.Mapping;
using RDeF.Mapping.Entities;
using RDeF.Mapping.Explicit;
using RDeF.Mapping.Visitors;

namespace Given_instance_of.UnmappedPropertyEventArgs_class
{
    [TestFixture]
    public class when_mapping_property
    {
        private Mock<IEntityContext> EntityContext { get; set; }

        private Mock<IExplicitMappings> ExplicitMappings { get; set; }

        private Mock<IConverterProvider> ConverterProvider { get; set; }

        private UnmappedPropertyEventArgs EventArgs { get; set; }

        public void TheTest()
        {
            EventArgs.OfEntity<IProduct>(config => config.WithProperty(typeof(string), "Test").MappedTo(new Iri("predicate")).WithDefaultConverter());
        }

        [Test]
        public void Should_set_the_dynamic_property_mapping()
        {
            ExplicitMappings.Verify(instance => instance.Set(
                It.Is<IEntityMapping>(mapping => mapping.Type == typeof(IProduct) && mapping.Properties.First().Name == "Test"),
                It.Is<Iri>(iri => iri == new Iri("subject"))));
        }

        [Test]
        public void Should_throw_on_attempt_to_configure_more_than_one_property()
        {
            EventArgs.Invoking(instance => instance.OfEntity<IProduct>(config => config
                .WithProperty(typeof(string), "Test").MappedTo(new Iri("predicate")).WithDefaultConverter()
                .WithProperty(typeof(string), "Another"))).Should().Throw<InvalidOperationException>();
        }

        [SetUp]
        public void Setup()
        {
            ExplicitMappings = new Mock<IExplicitMappings>(MockBehavior.Strict);
            EntityContext = new Mock<IEntityContext>(MockBehavior.Strict);
            ExplicitMappings.Setup(instance => instance.Set(It.IsAny<IEntityMapping>(), It.IsAny<Iri>()));
            ConverterProvider = new Mock<IConverterProvider>(MockBehavior.Strict);
            EventArgs = new UnmappedPropertyEventArgs(EntityContext.Object, new Statement(new Iri("subject"), new Iri("predicate"), "test"));
            EntityContextExtensions.ExplicitMappings[EntityContext.Object] = ExplicitMappings.Object;
            EntityContextExtensions.MappingVisitors = Array.Empty<IMappingProviderVisitor>();
            EntityContextExtensions.ConverterProvider = ConverterProvider.Object;
            EntityContextExtensions.QIriMappings = Array.Empty<QIriMapping>();
            TheTest();
        }

        [TearDown]
        public void Teardown()
        {
            EntityContextExtensions.ExplicitMappings.Remove(EntityContext.Object);
            EntityContextExtensions.MappingVisitors = null;
            EntityContextExtensions.ConverterProvider = null;
            EntityContextExtensions.QIriMappings = null;
        }
    }
}
