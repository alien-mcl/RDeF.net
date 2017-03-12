using System.Collections.Generic;

namespace RDeF.Entities
{
    internal interface IChangeDetector
    {
        void Process(Entity entity, ref IDictionary<IEntity, ISet<Statement>> retractedStatements, ref IDictionary<IEntity, ISet<Statement>> addedStatements);
    }
}
