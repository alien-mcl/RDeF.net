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

        public void Process(Entity entity, IDictionary<IEntity, ISet<Statement>> retractedStatements, IDictionary<IEntity, ISet<Statement>> addedStatements)
        {
            Process<Type>(entity, entity.OriginalTypes, entity.CastedTypes, retractedStatements, addedStatements);
            Process<MulticastPropertyValue>(entity, entity.OriginalValues, entity.PropertyValues, retractedStatements, addedStatements);
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

                AddStatements(ref retractedStatements, entity, originalValue);
            }

            foreach (var value in values.Except(matched))
            {
                AddStatements(ref addedStatements, entity, value);
            }
        }

        private void AddStatements<T>(ref IDictionary<IEntity, ISet<Statement>> statements, IEntity entity, T value)
        {
            var type = value as Type;
            if (type != null)
            {
                foreach (var @class in _mappingsRepository.FindEntityMappingFor(type).Classes)
                {
                    statements.EnsureKey(entity).Add(new Statement(entity.Iri, rdfs.type, @class.Term, @class.Graph));
                }

                return;
            }

            var propertyValue = value as MulticastPropertyValue;
            var propertyMapping = _mappingsRepository.FindPropertyMappingFor(propertyValue.Property);
            statements.EnsureKey(entity).Add(propertyMapping.ValueConverter.ConvertTo(entity.Iri, propertyMapping.Term, propertyValue.Value));
        }
    }
}
