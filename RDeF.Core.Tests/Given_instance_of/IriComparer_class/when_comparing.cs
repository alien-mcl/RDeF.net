using FluentAssertions;
using NUnit.Framework;
using RDeF.Entities;

namespace Given_instance_of.IriComparer_class
{
    [TestFixture]
    public class when_comparing
    {
        [Test]
        public void Should_consider_both_null_operands_as_equal()
        {
            IriComparer.Default.Compare(null, null).Should().Be(0);
        }

        [Test]
        public void Should_consider_left_null_operand_as_smaller()
        {
            IriComparer.Default.Compare(null, new Iri()).Should().Be(-1);
        }

        [Test]
        public void Should_consider_right_null_operand_as_smaller()
        {
            IriComparer.Default.Compare(new Iri(), null).Should().Be(1);
        }

        [Test]
        public void Should_Should_consider_same_iris_as_equal()
        {
            IriComparer.Default.Compare(new Iri("test"), new Iri("test")).Should().Be(0);
        }
    }
}
