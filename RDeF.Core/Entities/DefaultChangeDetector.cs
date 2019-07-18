using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RDeF.Collections;
using RDeF.Mapping;
using RDeF.Vocabularies;
using RollerCaster;

namespace RDeF.Entities
{
    internal class DefaultChangeDetector : IChangeDetector
    {
        private readonly IMappingsRepository _mappingsRepository;

        /// <summary>Initializes a new instance of the <see cref="DefaultChangeDetector" /> class.</summary>
        /// <param name="mappingsRepository">Mappings repository.</param>
        internal DefaultChangeDetector(IMappingsRepository mappingsRepository)
        {
            if (mappingsRepository == null)
            {
                throw new ArgumentNullException(nameof(mappingsRepository));
            }

            _mappingsRepository = mappingsRepository;
        }

        /// <inheritdoc />
        public void Process(Entity entity, IDictionary<IEntity, ISet<Statement>> retractedStatements, IDictionary<IEntity, ISet<Statement>> addedStatements)
        {
            Process(entity, entity.OriginalTypes, entity.CastedTypes, retractedStatements, addedStatements);
            Process(entity, entity.OriginalValues, entity.PropertyValues, retractedStatements, addedStatements);
        }

        /// <summary>Obtains an entity mapping.</summary>
        /// <param name="type">Type for which to obtain mapping.</param>
        /// <param name="owningEntity">Owning entity acting as an optional context when choosing mappings.</param>
        /// <returns>Entity mapping.</returns>
        protected virtual IEntityMapping GetEntityMapping(Type type, IEntity owningEntity)
        {
            return _mappingsRepository.FindEntityMappingFor(owningEntity, type);
        }

        /// <summary>Obtains a property mapping.</summary>
        /// <param name="property">Property for which to obtain mapping.</param>
        /// <param name="owningEntity">Owning entity acting as an optional context when choosing mappings.</param>
        /// <returns>Property mapping.</returns>
        protected virtual IPropertyMapping GetPropertyMapping(PropertyInfo property, IEntity owningEntity)
        {
            return _mappingsRepository.FindPropertyMappingFor(owningEntity, property);
        }

        private static void AddStatements(ISet<Statement> entityStatements, ICollectionMapping collectionMapping, IEntity entity, IEnumerable values)
        {
            if (collectionMapping.StoreAs == CollectionStorageModel.Simple)
            {
                AddStatements(entityStatements, collectionMapping, entity.Iri, values);
                return;
            }

            var iri = entity.Iri;
            var term = collectionMapping.Term;
            foreach (var value in values)
            {
                var auxIri = new Iri();
                entityStatements.Add(iri != entity.Iri ? new Statement(iri, rdf.rest, auxIri) : new Statement(iri, term, auxIri));
                var entityValue = value as IEntity;
                entityStatements.Add(entityValue != null
                    ? new Statement(auxIri, rdf.first, entityValue.Iri)
                    : collectionMapping.ValueConverter.ConvertTo(auxIri, rdf.first, value, collectionMapping.Graph));
                iri = auxIri;
            }

            entityStatements.Add(new Statement(iri, rdf.rest, rdf.nil));
        }

        private static void AddStatements(ISet<Statement> entityStatements, ICollectionMapping collectionMapping, Iri iri, IEnumerable values)
        {
            foreach (var value in values)
            {
                var entityValue = value as IEntity;
                entityStatements.Add(entityValue != null
                    ? new Statement(iri, collectionMapping.Term, entityValue.Iri)
                    : collectionMapping.ValueConverter.ConvertTo(iri, collectionMapping.Term, value, collectionMapping.Graph));
            }
        }

        private void Process<T>(
            Entity entity,
            IEnumerable<T> originalValues,
            IEnumerable<T> values,
            IDictionary<IEntity, ISet<Statement>> retractedStatements,
            IDictionary<IEntity, ISet<Statement>> addedStatements)
        {
            var matched = new HashSet<T>();
            foreach (var originalValue in originalValues)
            {
                bool isMatch = false;
                foreach (var value in values)
                {
                    if ((value.GetHashCode() != originalValue.GetHashCode()) || (!value.Equals(originalValue)))
                    {
                        continue;
                    }

                    matched.Add(value);
                    isMatch = true;
                    break;
                }

                if (isMatch)
                {
                    continue;
                }

                AddStatements(retractedStatements, entity, originalValue);
            }

            foreach (var value in values.Except(matched))
            {
                AddStatements(addedStatements, entity, value);
            }
        }

        private void AddStatements<T>(IDictionary<IEntity, ISet<Statement>> statements, IEntity entity, T value)
        {
            if (!AddTypeAssertion(statements, entity, value))
            {
                AddPropertyAssertion(statements, entity, value);
            }
        }

        private bool AddTypeAssertion<T>(IDictionary<IEntity, ISet<Statement>> statements, IEntity entity, T value)
        {
            bool result = false;
            var type = value as Type;
            if (type != null)
            {
                if (typeof(IEntity).IsAssignableFrom(type))
                {
                    var entityMapping = GetEntityMapping(type, entity);
                    if (entityMapping != null)
                    {
                        foreach (var @class in entityMapping.Classes)
                        {
                            statements.EnsureKey(entity).Add(new Statement(entity.Iri, rdf.type, @class.Term, @class.Graph));
                        }
                    }
                }

                result = true;
            }

            return result;
        }

        private void AddPropertyAssertion<T>(IDictionary<IEntity, ISet<Statement>> statements, IEntity entity, T value)
        {
            var propertyValue = value as MulticastPropertyValue;
            if (propertyValue?.Value != null)
            {
                var propertyMapping = GetPropertyMapping(propertyValue.Property, entity);
                if (propertyMapping != null)
                {
                    var collectionMapping = propertyMapping as ICollectionMapping;
                    if (collectionMapping != null)
                    {
                        AddStatements(statements.EnsureKey(entity), collectionMapping, entity, (IEnumerable)propertyValue.Value);
                        return;
                    }

                    var entityValue = propertyValue.Value as IEntity;
                    statements.EnsureKey(entity).Add(entityValue != null
                        ? new Statement(entity.Iri, propertyMapping.Term, entityValue.Iri, propertyMapping.Graph)
                        : propertyMapping.ValueConverter.ConvertTo(entity.Iri, propertyMapping.Term, propertyValue.Value, propertyMapping.Graph));
                }
            }
        }
    }
}
