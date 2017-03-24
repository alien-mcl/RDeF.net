using FluentAssertions;
using NUnit.Framework;
using RDeF.Entities;
using RDeF.Mapping;

namespace Given_instance_of.Iri_class
{
    [TestFixture]
    public class which_is_already_initialized
    {
        private QIriMapping QIriMapping { get; set; }

        [Test]
        public void Should_obtain_an_iri()
        {
            QIriMapping.Iri.Should().Be(new Iri("iri"));
        }

        [Test]
        public void Should_obtain_a_prefix()
        {
            QIriMapping.Prefix.Should().Be("prefix");
        }

        [SetUp]
        public void Setup()
        {
            QIriMapping = new QIriMapping("prefix", new Iri("iri"));
        }
    }
}
