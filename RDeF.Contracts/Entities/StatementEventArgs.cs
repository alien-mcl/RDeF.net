using System;
using System.Collections.Generic;

namespace RDeF.Entities
{
    /// <summary>Provides details about a statement associated with an event.</summary>
    public class StatementEventArgs : EventArgs
    {
        /// <summary>Initializes a new instance of the <see cref="StatementEventArgs" /> class.</summary>
        /// <param name="statement">Statement associated with the event.</param>
        public StatementEventArgs(Statement statement)
        {
            if (statement == null)
            {
                throw new ArgumentNullException(nameof(statement));
            }

            Statement = statement;
            AdditionalStatementsToAssert = new List<Statement>();
        }

        /// <summary>Gets the statement associated with the event.</summary>
        public Statement Statement { get; }

        /// <summary>Gets the collection of additional statements to assert.</summary>
        public ICollection<Statement> AdditionalStatementsToAssert { get; }
    }
}