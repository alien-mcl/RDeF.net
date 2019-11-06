using System;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Entities;

namespace Given_instance_of.Iri_class
{
    [TestFixture]
    public class when_concatenating
    {
        private static readonly Iri OperandA = new Iri("some:operandA");
        private static readonly Iri OperandB = new Iri(new Uri("operandB", UriKind.Relative));
        private static readonly Iri UriA = new Iri(new Uri("http://test.com/", UriKind.Absolute));
        private static readonly Iri UriB = new Iri(new Uri("test", UriKind.Relative));

        [Test]
        public void Should_throw_when_right_operand_is_a_blank_id()
        {
            OperandA.Invoking(left => { var result = left + new Iri(); })
                .Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void Should_throw_when_left_operand_is_a_blank_id()
        {
            OperandB.Invoking(right => { var result = new Iri() + right; })
                .Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void Should_return_null_for_both_null_operands()
        {
            ((Iri)null + (Iri)null).Should().BeNull();
        }

        [Test]
        public void Should_return_null_for_both_null_iri_and_string_operands()
        {
            ((Iri)null + (string)null).Should().BeNull();
        }

        [Test]
        public void Should_return_second_operand_for_first_null_operand()
        {
            ((Iri)null + OperandB).Should().Be(OperandB);
        }

        [Test]
        public void Should_return_first_operand_for_second_null_operand()
        {
            (OperandA + (Iri)null).Should().Be(OperandA);
        }

        [Test]
        public void Should_return_first_operand_for_second_null_string_operand()
        {
            (OperandA + (string)null).Should().Be(OperandA);
        }

        [Test]
        public void Should_concatenate_both_operands()
        {
            (OperandA + OperandB).ToString().Should().Be(OperandA.ToString() + OperandB.ToString());
        }

        [Test]
        public void Should_concatenate_both_Uri_operands()
        {
            (UriA + UriB).ToString().Should().Be(UriA.ToString() + UriB.ToString());
        }

        [Test]
        public void Should_throw_for_both_Uris_are_absolute()
        {
            UriA.Invoking(uriA => { var uri = uriA + UriA; })
                .Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void Should_throw_for_no_iri_and_string_operands()
        {
            "test".Invoking(operandB => { var uri = (Iri)null + operandB; })
                .Should().Throw<InvalidOperationException>();
        }
    }
}
