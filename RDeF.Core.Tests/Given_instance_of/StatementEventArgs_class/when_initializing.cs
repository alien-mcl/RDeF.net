using System;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Entities;

namespace Given_instance_of.StatementEventArgs_class
{
    public class when_initializing
    {
        [Test]
        public void Should_throw_when_no_statement_is_given()
        {
            ((StatementEventArgs)null).Invoking(_ => new StatementEventArgs(null))
                .Should().Throw<ArgumentNullException>()
                .Which.ParamName.Should().Be("statement");
        }
    }
}
