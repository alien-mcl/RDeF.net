using System.Collections.Generic;
using RDeF.Mapping;
using RDeF.Vocabularies;
using RollerCaster;

namespace RDeF.Entities
{
    internal static class EntityExtensions
    {
        internal static void SetProperty(this Entity entity, Statement statement, IPropertyMapping propertyMapping, EntityInitializationContext context)
        {
            var value = propertyMapping.ValueConverter.ConvertFrom(statement);
            entity.SetPropertyInternal(propertyMapping.EntityMapping.Type, propertyMapping.Name, value);
            var otherEntity = value as IEntity;
            if (otherEntity != null)
            {
                context.EntitiesCreated.Add((Entity)otherEntity.Unwrap());
            }
        }

        internal static void SetList(this Entity entity, Iri head, ICollectionMapping collectionMapping, EntityInitializationContext context)
        {
            Iri previousHead = null;
            while (true)
            {
                ISet<Statement> statements;
                if ((previousHead == head) || (!context.EntityStatements.TryGetValue(head, out statements)))
                {
                    break;
                }

                context.EntityStatements.Remove(previousHead = head);
                foreach (var statement in statements)
                {
                    if (statement.Predicate == rdf.first)
                    {
                        entity.SetProperty(statement, collectionMapping, context);
                    }
                    else if (statement.Predicate == rdf.last)
                    {
                        head = statement.Object;
                    }
                }
            }
        }

        internal static void InitializeLists(this Entity entity, EntityInitializationContext context)
        {
            Dictionary<Iri, ICollectionMapping> linkedLists;
            if (!context.LinkedLists.TryGetValue(entity.Iri, out linkedLists))
            {
                return;
            }

            foreach (var head in linkedLists)
            {
                entity.SetList(head.Key, head.Value, context);
            }

            context.LinkedLists.Remove(entity.Iri);
        }
    }
}
