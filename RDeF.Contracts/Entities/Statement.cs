using System;
using System.Diagnostics;

namespace RDeF.Entities
{
    /// <summary>Represents an RDF statement.</summary>
    [DebuggerDisplay("<{Subject,nq}> <{Predicate,nq}> {Object!=null?\"<\"+Object.ToString()+\">\":\"\\\"\"+Value+\"\\\"\",nq}")]
    public class Statement
    {
        /// <summary>Initializes a new instance of the <see cref="Statement"/> class.</summary>
        /// <param name="subject">Subject of the statement.</param>
        /// <param name="predicate">Predicate of the statement..</param>
        /// <param name="object">Related object.</param>
        /// <param name="graph">Optional graph.</param>
        public Statement(Iri subject, Iri predicate, Iri @object, Iri graph = null)
        {
            if (subject == null)
            {
                throw new ArgumentNullException(nameof(subject));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            if (@object == null)
            {
                throw new ArgumentNullException(nameof(@object));
            }

            Graph = graph;
            Subject = subject;
            Predicate = predicate;
            Object = @object;
        }

        /// <summary>Initializes a new instance of the <see cref="Statement"/> class.</summary>
        /// <param name="subject">Subject of the statement.</param>
        /// <param name="predicate">Predicate of the statement..</param>
        /// <param name="value">Literal value of the statement.</param>
        /// <param name="dataType">Type of the literal value.</param>
        /// <param name="graph">Optional graph.</param>
        public Statement(Iri subject, Iri predicate, string value, Iri dataType = null, Iri graph = null)
        {
            if (subject == null)
            {
                throw new ArgumentNullException(nameof(subject));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Graph = graph;
            Subject = subject;
            Predicate = predicate;
            Value = value;
            DataType = dataType;
        }

        /// <summary>Initializes a new instance of the <see cref="Statement"/> class.</summary>
        /// <param name="subject">Subject of the statement.</param>
        /// <param name="predicate">Predicate of the statement..</param>
        /// <param name="value">Literal value of the statement.</param>
        /// <param name="language">Language of the literal value.</param>
        /// <param name="graph">Optional graph.</param>
        public Statement(Iri subject, Iri predicate, string value, string language, Iri graph = null)
        {
            if (subject == null)
            {
                throw new ArgumentNullException(nameof(subject));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (language == null)
            {
                throw new ArgumentNullException(nameof(language));
            }

            Graph = graph;
            Subject = subject;
            Predicate = predicate;
            Value = value;
            Language = language;
        }

        /// <summary>Gets the graph.</summary>
        public Iri Graph { get; }

        /// <summary>Gets the subject.</summary>
        public Iri Subject { get; }

        /// <summary>Gets the predicate.</summary>
        public Iri Predicate { get; }

        /// <summary>Gets the object.</summary>
        /// <remarks>If the <see cref="Statement.Value" /> is set, this will return <b>null</b>.</remarks>
        public Iri Object { get; }

        /// <summary>Gets the literal value type.</summary>
        /// <remarks>In case the type is not set, this property will return <b>null</b>.</remarks>
        public Iri DataType { get; }

        /// <summary>Gets the literal value.</summary>
        /// <remarks>If the <see cref="Statement.Object" /> is set, this will return <b>null</b>.</remarks>
        public string Value { get; }

        /// <summary>Gets the language of the literal.</summary>
        /// <remarks>In case the language is not set, this property will return <b>null</b>.</remarks>
        public string Language { get; }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            Statement anotherStatement = obj as Statement;
            if (anotherStatement == null)
            {
                return false;
            }

            return Equals(Graph, anotherStatement.Graph) &&
                   Equals(Subject, anotherStatement.Subject) &&
                   Equals(Predicate, anotherStatement.Predicate) &&
                   Equals(Object, anotherStatement.Object) &&
                   Equals(DataType, anotherStatement.DataType) &&
                   Equals(Value, anotherStatement.Value) &&
                   Equals(Language, anotherStatement.Language);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Graph?.GetHashCode() ?? 0 ^
                   Subject.GetHashCode() ^
                   Predicate.GetHashCode() ^
                   Object?.GetHashCode() ?? 0 ^
                   DataType?.GetHashCode() ?? 0 ^
                   Value?.GetHashCode() ?? 0 ^
                   Language?.GetHashCode() ?? 0;
        }
    }
}