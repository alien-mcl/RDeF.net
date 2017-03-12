using System;
using System.Collections.Generic;
using System.Linq;
using RDeF.Collections;
using RDeF.Mapping;
using RDeF.Vocabularies;
using RollerCaster;

namespace RDeF.Entities
{
    internal class DefaultChangeDetector : IChangeDetector
    {
        private readonly IMappingsRepository _mappingsRepository;

        internal DefaultChangeDetector(IMappingsRepository mappingsRepository)
        {
            _mappingsRepository = mappingsRepository;
        }

        public void Process(Entity entity, ref IDictionary<IEntity, ISet<Statement>> retractedStatements, ref IDictionary<IEntity, ISet<Statement>> addedStatements)
        {
            ProcessTypes(entity, ref retractedStatements, ref addedStatements);
            ProcessProperties(entity, ref retractedStatements, ref addedStatements);
        }

        private void ProcessTypes(Entity entity, ref IDictionary<IEntity, ISet<Statement>> retractedStatements, ref IDictionary<IEntity, ISet<Statement>> addedStatements)
        {
            var matchedTypes = new HashSet<Type>();
            foreach (var originalType in entity.OriginalTypes)
            {
                bool isMatch = false;
                foreach (var propertyValue in entity.CastedTypes)
                {
                    if ((propertyValue.GetHashCode() != originalType.GetHashCode()) || (!propertyValue.Equals(originalType)))
                    {
                        continue;
                    }

                    matchedTypes.Add(propertyValue);
                    isMatch = true;
                    break;
                }

                if (isMatch)
                {
                    continue;
                }

                AddStatements(ref retractedStatements, entity, originalType);
            }

            foreach (var type in entity.CastedTypes.Except(matchedTypes))
            {
                AddStatements(ref addedStatements, entity, type);
            }
        }

        private void ProcessProperties(Entity entity, ref IDictionary<IEntity, ISet<Statement>> retractedStatements, ref IDictionary<IEntity, ISet<Statement>> addedStatements)
        {
            bool isFirstPass = true;
            var currentProperties = new HashSet<MulticastPropertyValue>();
            var matchedProperties = new HashSet<MulticastPropertyValue>();
            foreach (var originalPropertyValue in entity.OriginalValues)
            {
                bool isMatch = false;
                foreach (var propertyValue in (isFirstPass ? (IEnumerable<MulticastPropertyValue>)entity.PropertyValues : currentProperties))
                {
                    currentProperties.AddIf(propertyValue, isFirstPass);
                    if ((propertyValue.GetHashCode() != originalPropertyValue.GetHashCode()) || (!propertyValue.Equals(originalPropertyValue)))
                    {
                        continue;
                    }

                    matchedProperties.Add(propertyValue);
                    isMatch = true;
                    break;
                }

                isFirstPass = false;
                if (isMatch)
                {
                    continue;
                }

                AddStatements(ref retractedStatements, entity, originalPropertyValue);
            }

            foreach (var propertyValue in currentProperties.Except(matchedProperties))
            {
                AddStatements(ref addedStatements, entity, propertyValue);
            }
        }

        private void AddStatements(ref IDictionary<IEntity, ISet<Statement>> statements, IEntity entity, MulticastPropertyValue propertyValue)
        {
            var propertyMapping = _mappingsRepository.FindPropertyMappingFor(propertyValue.Property);
            statements.EnsureKey(entity).Add(propertyMapping.ValueConverter.ConvertTo(entity.Iri, propertyMapping.Predicate, propertyValue.Value));
        }

        private void AddStatements(ref IDictionary<IEntity, ISet<Statement>> statements, IEntity entity, Type castedType)
        {
            foreach (var @class in _mappingsRepository.FindEntityMappingFor(castedType).Classes)
            {
                statements.EnsureKey(entity).Add(new Statement(entity.Iri, rdfs.type, @class));
            }
        }
    }
}
