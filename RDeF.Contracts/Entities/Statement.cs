using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using RDeF.Vocabularies;

namespace RDeF.Entities
{
    /// <summary>Represents an RDF statement.</summary>
    [DebuggerDisplay("{AsString,nq}")]
    public class Statement
    {
        private string _asString;

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

        [ExcludeFromCodeCoverage]
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "Debugging facility.")]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Debugging facility.")]
        internal string AsString
        {
            get
            {
                return _asString ??
                    (_asString = String.Format(
                        "{0} {1} {2}",
                        Subject.AsString,
                        Predicate.AsString,
                        Object?.AsString ?? Value + (DataType != null ? $"^^{DataType.AsString}" : !String.IsNullOrEmpty(Language) ? $@"{Language}" : String.Empty)));
            }
        }

        /// <summary>Comparse two <see cref="Statement" />s for equality.</summary>
        /// <param name="left">Left operand.</param>
        /// <param name="right">Right operand.</param>
        /// <returns><b>true</b> if both operands has equal components; otherwise <b>false</b>.</returns>
        public static bool operator ==(Statement left, Statement right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if ((ReferenceEquals(left, null)) || (ReferenceEquals(right, null)))
            {
                return false;
            }

            return Equals(left, right);
        }

        /// <summary>Comparse two <see cref="Statement" />s for inequality.</summary>
        /// <param name="left">Left operand.</param>
        /// <param name="right">Right operand.</param>
        /// <returns><b>true</b> if both operands has a single component different; otherwise <b>false</b>.</returns>
        public static bool operator !=(Statement left, Statement right)
        {
            if (ReferenceEquals(left, right))
            {
                return false;
            }

            if ((ReferenceEquals(left, null)) || (ReferenceEquals(right, null)))
            {
                return true;
            }

            return !Equals(left, right);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            var anotherStatement = obj as Statement;
            return anotherStatement != null && Equals(this, anotherStatement);
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

        private static bool Equals(Statement left, Statement right)
        {
            if (!Equals(left.Graph, right.Graph) ||
                !Equals(left.Subject, right.Subject) ||
                !Equals(left.Predicate, right.Predicate))
            {
                return false;
            }

            if ((!ReferenceEquals(left.Object, null)) && (!ReferenceEquals(right.Object, null)))
            {
                return Equals(left.Object, right.Object);
            }

            if (!ReferenceEquals(left.Value, null) && (!ReferenceEquals(right.Value, null)))
            {
                return Equals(left.Value, right.Value) &&
                       Equals(left.Language, right.Language) &&
                       Equals(left.DataType ?? xsd.@string, right.DataType ?? xsd.@string);
            }

            return false;
        }
    }
}