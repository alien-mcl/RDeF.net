using System.Collections.Generic;

namespace RDeF.Entities
{
    /// <summary>Describes an abstract facility for detecking entity changes.</summary>
    internal interface IChangeDetector
    {
        /// <summary>Instructs to process a given <paramref name="entity" /> for change detection.</summary>
        /// <param name="entity">Entity to be processed.</param>
        /// <param name="retractedStatements">Map that will receive retracted statements for entities being processed.</param>
        /// <param name="addedStatements">Map that will receive added statements for entities being processed.</param>
        void Process(Entity entity, IDictionary<IEntity, ISet<Statement>> retractedStatements, IDictionary<IEntity, ISet<Statement>> addedStatements);
    }
}
