using FluentAssertions;
using NUnit.Framework;

namespace Given_instance_of.Entity_class
{
    [TestFixture]
    public class when_initializing : EntityTest
    {
        [Test]
        public void Should_set_the_context_correctly()
        {
            Entity.Context.Should().Be(Context.Object);
        }

        [Test]
        public void Should_set_the_Iri_correctly()
        {
            Entity.Iri.Should().Be(Iri);
        }
    }
}
