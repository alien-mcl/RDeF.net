using System.Collections.Concurrent;
using System.Collections.Generic;
using RDeF.Mapping;

namespace RDeF.Entities
{
    internal class EntityInitializationContext
    {
        internal EntityInitializationContext()
        {
            EntityStatements = new ConcurrentDictionary<Iri, ISet<Statement>>();
            LinkedLists = new ConcurrentDictionary<Iri, Dictionary<Iri, ICollectionMapping>>();
            EntitiesCreated = new HashSet<Entity>();
        }

        internal IDictionary<Iri, ISet<Statement>> EntityStatements { get; }

        internal IDictionary<Iri, Dictionary<Iri, ICollectionMapping>> LinkedLists { get; }

        internal ISet<Entity> EntitiesCreated { get; }
    }
}
