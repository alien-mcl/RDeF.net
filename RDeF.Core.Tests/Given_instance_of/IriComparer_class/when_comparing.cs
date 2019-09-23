using FluentAssertions;
using NUnit.Framework;
using RDeF.Entities;

namespace Given_instance_of.IriComparer_class
{
    [TestFixture]
    public class when_comparing
    {
        private Iri Iri { get; set; }

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
        public void Should_consider_same_iris_as_equal()
        {
            IriComparer.Default.Compare(new Iri("test"), new Iri("test")).Should().Be(0);
        }

        [Test]
        public void Should_consider_same_iri_instances_as_equal()
        {
            IriComparer.Default.Equals(Iri = new Iri("test"), Iri).Should().BeTrue();
        }

        [Test]
        public void Should_consider_nulls_as_equal()
        {
            IriComparer.Default.Equals(null, null).Should().BeTrue();
        }

        [Test]
        public void Should_consider_iris_with_same_identifier_as_equal()
        {
            IriComparer.Default.Equals(new Iri("test"), new Iri("test")).Should().BeTrue();
        }

        [Test]
        public void Should_obtain_underlying_iris_identifier_as_its_hash_code()
        {
            IriComparer.Default.GetHashCode(new Iri("test")).Should().Be("_:test".GetHashCode());
        }
    }
}
