using FluentAssertions;
using FluentAssertions.Primitives;
using RDeF.Entities;

namespace RDeF
{
    internal static class AssertionsExtensions
    {
        internal static void MatchLiteralValueOf(this ObjectAssertions statement, string expectedValue, Iri expectedDataType = null)
        {
            var isStatement = statement.BeOfType<Statement>();
            isStatement.Which.Value.Should().Be(expectedValue);
            isStatement.Which.DataType.Should().Be(expectedDataType);
        }
    }
}
