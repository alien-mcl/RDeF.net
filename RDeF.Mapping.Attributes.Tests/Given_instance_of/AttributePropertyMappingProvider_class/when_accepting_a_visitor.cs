using System;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RDeF.Mapping.Providers;
using RDeF.Mapping.Visitors;
using PropertyAttribute = RDeF.Mapping.Attributes.PropertyAttribute;

namespace Given_instance_of.AttributePropertyMappingProvider_class
{
    [TestFixture]
    public class when_accepting_a_visitor : AttributePropertyMappingProviderTest
    {
        private Mock<IMappingProviderVisitor> Visitor { get; set; }

        public override void TheTest()
        {
            Provider.Accept(Visitor.Object);
        }

        [Test]
        public void Should_visit_that_visitor()
        {
            Visitor.Verify(instance => instance.Visit(Provider), Times.Once);
        }

        [Test]
        public void Should_throw_if_no_visitor_is_given()
        {
            Provider.Invoking(instance => instance.Accept(null)).ShouldThrow<ArgumentNullException>();
        }

        protected override void ScenarioSetup()
        {
            Visitor = new Mock<IMappingProviderVisitor>(MockBehavior.Strict);
            Visitor.Setup(instance => instance.Visit(It.IsAny<IPropertyMappingProvider>()));
            Provider = AttributePropertyMappingProvider.FromAttribute(EntityType, Property, new PropertyAttribute() { Iri = "test" });
        }
    }
}
