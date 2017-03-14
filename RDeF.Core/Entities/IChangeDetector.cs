using System.Collections.Generic;

namespace RDeF.Entities
{
    internal interface IChangeDetector
    {
        void Process(Entity entity, IDictionary<IEntity, ISet<Statement>> retractedStatements, IDictionary<IEntity, ISet<Statement>> addedStatements);
    }
}
