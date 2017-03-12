using FluentAssertions;
using NUnit.Framework;
using RDeF.Entities;

namespace Given_instance_of.Iri_class
{
    [TestFixture]
    public class when_comparing
    {
        [Test]
        public void Should_confirm_equality_of_two_null_operands()
        {
            ((Iri)null == (Iri)null).Should().BeTrue();
        }

        [Test]
        public void Should_confirm_equality_of_two_same_iris_operands()
        {
            (new Iri("test") == new Iri("test")).Should().BeTrue();
        }

        [Test]
        public void Should_confirm_inequality_of_null_and_not_null_iri_operands()
        {
            (new Iri("test") != (Iri)null).Should().BeTrue();
        }

        [Test]
        public void Should_confirm_inequality_of_two_different_iris_operands()
        {
            (new Iri("1") != new Iri("2")).Should().BeTrue();
        }
    }
}
