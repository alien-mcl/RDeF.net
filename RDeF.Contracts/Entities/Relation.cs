using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace RDeF.Entities
{
    /// <summary>Provides a connection between it's owner being a subject and other resources in relation through unmapped predicates.</summary>
    [DebuggerDisplay("{" + nameof(AsString) + ",nq}")]
    public class Relation : IEquatable<Relation>
    {
        private const int Salt = 113;
        private readonly int _hashCode;
        private IEntity _object;
        private string _asString;

        /// <summary>Initializes a new instance of the <see cref="Relation" /> class.</summary>
        /// <param name="predicate">Unmapped predicate</param>
        /// <param name="object">Object being in relation.</param>
        /// <param name="graph">Iri of the graph this relation is defined in.</param>
        public Relation(Iri predicate, IEntity @object, Iri graph = null)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            if (@object == null)
            {
                throw new ArgumentNullException(nameof(@object));
            }

            if (@object.Iri == null)
            {
                throw new ArgumentOutOfRangeException(nameof(@object));
            }

            Predicate = predicate;
            _object = @object;
            Value = null;
            Graph = graph ?? Iri.DefaultGraph;
            unchecked
            {
                _hashCode = predicate.GetHashCode() ^ @object.Iri.GetHashCode();
                if (graph != null)
                {
                    _hashCode ^= graph.GetHashCode();
                }

                _hashCode *= Salt;
            }
        }

        /// <summary>Initializes a new instance of the <see cref="Relation" /> class.</summary>
        /// <param name="predicate">Unmapped predicate</param>
        /// <param name="value">Literal value.</param>
        /// <param name="graph">Iri of the graph this relation is defined in.</param>
        public Relation(Iri predicate, object value, Iri graph = null)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Predicate = predicate;
            _object = null;
            Value = value;
            Graph = graph ?? Iri.DefaultGraph;
            unchecked
            {
                _hashCode = predicate.GetHashCode() ^ value.GetHashCode();
                if (graph != null)
                {
                    _hashCode ^= graph.GetHashCode();
                }

                _hashCode *= Salt;
            }
        }

        /// <summary>Gets the unmapped predicate.</summary>
        public Iri Predicate { get; }

        /// <summary>Gets the object being in relation.</summary>
        /// <remarks>This property is exclusive regarding the <see cref="Value" /> property.</remarks>
        public IEntity Object
        {
            get { return _object; }
        }

        /// <summary>Gets the literal value.</summary>
        /// <remarks>This property is exclusive regarding the <see cref="Object" /> property.</remarks>
        public object Value { get; }

        /// <summary>Gets the Iri of the graph this relation is defined in.</summary>
        /// <remarks>This can be <i>null</i> meaning that a default graph is used.</remarks>
        public Iri Graph { get; }

        [ExcludeFromCodeCoverage]
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "Debugging facility.")]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Debugging facility.")]
        internal string AsString
        {
            get
            {
                return _asString ??
                    (_asString = String.Format(
                        "{0} {1}{2}",
                        Predicate.AsString,
                        Object?.Iri?.AsString ?? Value,
                        Graph != Iri.DefaultGraph ? $" {Graph.AsString}" : String.Empty));
            }
        }

        /// <summary>CHecks for equality between two <see cref="Relation" />s.</summary>
        /// <param name="left">Left operand.</param>
        /// <param name="right">Right operand.</param>
        /// <returns><i>true</i> in case all properties are equal; otherwise <i>false</i>.</returns>
        public static bool operator ==(Relation left, Relation right)
        {
            return (ReferenceEquals(left, null) && ReferenceEquals(right, null))
                || (!ReferenceEquals(left, null) && !ReferenceEquals(right, null) && left.Equals(right));
        }

        /// <summary>CHecks for inequality between two <see cref="Relation" />s.</summary>
        /// <param name="left">Left operand.</param>
        /// <param name="right">Right operand.</param>
        /// <returns><i>true</i> in case any of the properties are different; otherwise <i>false</i>.</returns>
        public static bool operator !=(Relation left, Relation right)
        {
            return (ReferenceEquals(left, null) && !ReferenceEquals(right, null))
                || (!ReferenceEquals(left, null) && ReferenceEquals(right, null))
                || (!ReferenceEquals(left, null) && !ReferenceEquals(right, null) && !left.Equals(right));
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return _hashCode;
        }

        /// <inheritdoc />
        public bool Equals(Relation other)
        {
            return other != null
                && Equals(Predicate, other.Predicate)
                && Equals(_object?.Iri, other._object?.Iri)
                && Equals(Value, other.Value)
                && Equals(Graph, other.Graph);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            Relation relation = obj as Relation;
            return relation != null && Equals(relation);
        }

        internal Relation Initialize(IEntity entity)
        {
            _object = entity;
            return this;
        }
    }
}