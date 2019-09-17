using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using RDeF.Collections;
using RDeF.Mapping;
using RDeF.Vocabularies;
using RollerCaster;
using RollerCaster.Reflection;

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
            var result = new HashSet<Iri>();
            foreach (var type in proxy.CastedTypes)
            {
                var entityMapping = entity.Context.Mappings.FindEntityMappingFor(entity, type);
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
            return entity.Is(new[] { @class });
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
            return classes != null
                && entity.ActLike<ITypedEntity>().Type.Union(entity.GetTypes())
                    .Join(classes, _ => _, _ => _, (outer, inner) => inner).Count() == classes.Count();
        }

        internal static void SetProperty(this Entity entity, Statement statement, IPropertyMapping propertyMapping, EntityInitializationContext context)
        {
            object value;
            if (statement.Object != null && propertyMapping.ValueConverter == null)
            {
                var otherEntity = (IEntity)((DefaultEntityContext)entity.Context)
                    .CreateInternal(statement.Object, false, CancellationToken.None)
                    .Result.ActLike(propertyMapping.ReturnType.GetItemType());
                context.EntitiesCreated.Add(otherEntity.UnwrapEntity());
                value = otherEntity;
            }
            else
            {
                value = propertyMapping.ValueConverter.ConvertFrom(statement);
            }

            entity.SetPropertyInternal(propertyMapping.PropertyInfo, value, null);
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

        internal static void SetUnmappedProperty(this Entity entity, Statement statement, IEnumerable<ILiteralConverter> literalConverters)
        {
            Relation relation;
            if (statement.Object != null)
            {
                var relatedEntity = new LazyLoadedEntity(entity.Context, statement.Object);
                relation = new Relation(statement.Predicate, relatedEntity, statement.Graph);
                relatedEntity.Relation = relation;
            }
            else
            {
                ILiteralConverter converter = null;
                if (statement.DataType != null)
                {
                    converter = literalConverters.FirstOrDefault(_ => _.SupportedDataTypes.Any(type => type == statement.DataType));
                }

                var value = converter != null ? converter.ConvertFrom(statement) : statement.Value;
                relation = new Relation(statement.Predicate, value, statement.Graph);
            }

            entity.UnmappedRelationsSet.Add(relation);
        }

        private static Entity UnwrapEntity(this IEntity entity)
        {
            return (Entity)entity.Unwrap();
        }
    }
}
