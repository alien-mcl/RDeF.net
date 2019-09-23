using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using RDeF.Entities;
using VDS.RDF;

namespace RDeF.RDF
{
    internal class TripleCollection : BaseTripleCollection
    {
        private readonly IEnumerable<Statement> _statements;
        private readonly NodeFactory _nodeFactory;

        internal TripleCollection(IEnumerable<Statement> statements)
        {
            _statements = statements;
            _nodeFactory = new NodeFactory();
        }
        
        /// <inheritdoc />
        public override int Count
        {
            get { return _statements.Count(); }
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public override IEnumerable<INode> ObjectNodes
        {
            get { return Array.Empty<INode>(); }
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public override IEnumerable<INode> PredicateNodes
        {
            get { return Array.Empty<INode>(); }
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public override IEnumerable<INode> SubjectNodes
        {
            get { return Array.Empty<INode>(); }
        }
        
        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public override Triple this[Triple t]
        {
            get { return null; }
        }
                
        /// <inheritdoc />
        public override IEnumerator<Triple> GetEnumerator()
        {
            return _statements.Select(_ => _.ToTriple(_nodeFactory)).GetEnumerator();
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            //// Nothing to dispose.
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        public override bool Contains(Triple t)
        {
            return false;
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        protected override bool Add(Triple t)
        {
            return false;
        }

        /// <inheritdoc />
        [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "This API won't be ever used.")]
        [ExcludeFromCodeCoverage]
        protected override bool Delete(Triple t)
        {
            return false;
        }
    }
}
