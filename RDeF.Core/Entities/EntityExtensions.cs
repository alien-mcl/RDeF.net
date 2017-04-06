using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using RDeF.Mapping;
using RDeF.Vocabularies;
using RollerCaster;

namespace RDeF.Entities
{
    /// <summary>Exposes useful <see cref="IEntity" /> extensions.</summary>
    public static class EntityExtensions
    {
        /// <summary>Obtains a collection of RDF classes a given <paramref name="entity" /> is mapped to.</summary>
        /// <param name="entity">Entity for which to obtain classes.</param>
        /// <returns>Collection of Iri of RDF classes a given <paramref name="entity" /> is mapped to.</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "Invalid argument will be rejected in the first line of this method.")]
        public static IEnumerable<Iri> GetTypes(this IEntity entity)
        {
            var proxy = entity.Unwrap();
            var result = new List<Iri>();
            foreach (var type in proxy.CastedTypes)
            {
                var entityMapping = entity.Context.Mappings.FindEntityMappingFor(type);
                if (entityMapping != null)
                {
                    result.AddRange(entityMapping.Classes.Select(@class => @class.Term));
                }
            }

            return result;
        }

        /// <summary>Checks whether a given <paramref name="entity" /> is of the given <paramref name="class" />.</summary>
        /// <param name="entity">Entity to check.</param>
        /// <param name="class">Class to search for.</param>
        /// <returns><b>true</b> if a given <paramref name="entity" /> is mapped to the <paramref name="class" />; otherwise <b>false</b>.</returns>
        public static bool Is(this IEntity entity, Iri @class)
        {
            return entity.GetTypes().Contains(@class);
        }

        /// <summary>Checks whether a given <paramref name="entity" /> is of all of the given <paramref name="classes" />.</summary>
        /// <param name="entity">Entity to check.</param>
        /// <param name="classes">Classes to search for.</param>
        /// <returns><b>true</b> if a given <paramref name="entity" /> is mapped to all of the <paramref name="classes" />; otherwise <b>false</b>.</returns>
        public static bool Is(this IEntity entity, params Iri[] classes)
        {
            return entity.Is((IEnumerable<Iri>)classes);
        }

        /// <summary>Checks whether a given <paramref name="entity" /> is of all of the given <paramref name="classes" />.</summary>
        /// <param name="entity">Entity to check.</param>
        /// <param name="classes">Classes to search for.</param>
        /// <returns><b>true</b> if a given <paramref name="entity" /> is mapped to all of the <paramref name="classes" />; otherwise <b>false</b>.</returns>
        public static bool Is(this IEntity entity, IEnumerable<Iri> classes)
        {
            return (classes != null) && (entity.GetTypes().Join(classes, outer => outer, inner => inner, (outer, inner) => inner).Count() == classes.Count());
        }

        internal static void SetProperty(this Entity entity, Statement statement, IPropertyMapping propertyMapping, EntityInitializationContext context)
        {
            var value = propertyMapping.ValueConverter.ConvertFrom(statement);
            entity.SetPropertyInternal(propertyMapping.EntityMapping.Type, propertyMapping.Name, value);
            var otherEntity = value as IEntity;
            if (otherEntity != null)
            {
                context.EntitiesCreated.Add(otherEntity.UnwrapEntity());
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
                    else if (statement.Predicate == rdf.rest)
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

        private static Entity UnwrapEntity(this IEntity entity)
        {
            return (Entity)entity.Unwrap();
        }
    }
}
