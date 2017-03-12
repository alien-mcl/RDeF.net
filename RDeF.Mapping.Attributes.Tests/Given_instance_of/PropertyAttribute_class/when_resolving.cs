using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Entities;
using RDeF.Mapping;
using PropertyAttribute = RDeF.Mapping.Attributes.PropertyAttribute;

namespace Given_instance_of.PropertyAttribute_class
{
    [TestFixture]
    public class when_resolving : PropertyAttributeTest
    {
        private const string AbsoluteIri = "absolute-iri";
        private static readonly IEnumerable<QIriMapping> QIriMappings = new[] { new QIriMapping(ExpectedPrefix, new Iri(AbsoluteIri)) };

        private Iri Result { get; set; }

        public override void TheTest()
        {
            Result = Attribute.Resolve(QIriMappings);
        }

        [Test]
        public void Should_resolve_to_an_absolute_Iri()
        {
            Result.ToString().Should().Be(AbsoluteIri + ExpectedTerm);
        }

        [Test]
        public void Should_throw_when_no_prefix_mapping_is_provided()
        {
            Attribute.Invoking(attribute => attribute.Resolve(new QIriMapping[0])).ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void Should_resolve_an_absolute_Iri_to_itself()
        {
            new PropertyAttribute(AbsoluteIri).Resolve(new QIriMapping[0]).ToString().Should().Be(AbsoluteIri);
        }
    }
}
