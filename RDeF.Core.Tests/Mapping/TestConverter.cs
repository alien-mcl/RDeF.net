using RDeF.Entities;

namespace RDeF.Mapping
{
    /// <summary>Test converter.</summary>
    public class TestConverter : IConverter
    {
        /// <inheritdoc />
        public bool Equals(IConverter other)
        {
            return other is TestConverter;
        }

        /// <inheritdoc />
        public object ConvertFrom(Statement statement)
        {
            return statement.Value;
        }

        /// <inheritdoc />
        public Statement ConvertTo(Iri subject, Iri predicate, object value, Iri graph = null)
        {
            return new Statement(subject, predicate, value.ToString());
        }
    }
}
