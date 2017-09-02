using System;
using RDeF.Mapping;

namespace RDeF.Entities
{
    /// <summary>Describes an event arguments of an unmapped entity property.</summary>
    public class UnmappedPropertyEventArgs : EventArgs
    {
        internal UnmappedPropertyEventArgs(IEntityContext entityContext, Statement statement)
        {
            Statement = statement;
            EntityContext = entityContext;
        }

        /// <summary>Gets a statement containing an unmapped predicate.</summary>
        public Statement Statement { get; }

        internal IEntityContext EntityContext { get; set; }

        internal IPropertyMapping PropertyMapping { get; set; }
    }
}