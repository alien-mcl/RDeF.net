using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RDeF.Entities;
using RDeF.Vocabularies;

namespace RDeF.Collections
{
    /// <summary>Provides a specialized <see cref="ICollection{Statement}" /> that prioritizies rdf:type predicates over other statements.</summary>
    public sealed class TypePrioritizingStatementCollection : ICollection<Statement>
    {
        private readonly ICollection<Statement> _typeAssertions = new List<Statement>();
        private readonly ICollection<Statement> _statements = new List<Statement>();

        /// <inheritdoc />>
        public int Count { get { return _typeAssertions.Count + _statements.Count; } }

        /// <inheritdoc />>
        public bool IsReadOnly { get { return false; } }

        /// <inheritdoc />>
        public IEnumerator<Statement> GetEnumerator()
        {
            return _typeAssertions.Concat(_statements).GetEnumerator();
        }

        /// <inheritdoc />>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc />>
        public void Add(Statement item)
        {
            if (item != null)
            {
                (item.Predicate == rdf.type ? _typeAssertions : _statements).Add(item);
            }
        }

        /// <inheritdoc />>
        public void Clear()
        {
            _typeAssertions.Clear();
            _statements.Clear();
        }

        /// <inheritdoc />>
        public bool Contains(Statement item)
        {
            return _typeAssertions.Contains(item) || _statements.Contains(item);
        }

        /// <inheritdoc />>
        void ICollection<Statement>.CopyTo(Statement[] array, int arrayIndex)
        {
            _typeAssertions.CopyTo(array, arrayIndex);
            _statements.CopyTo(array, arrayIndex + _typeAssertions.Count);
        }

        /// <inheritdoc />>
        public bool Remove(Statement item)
        {
            return item != null && (item.Predicate == rdf.type ? _typeAssertions : _statements).Remove(item);
        }
    }
}
