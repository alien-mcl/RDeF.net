using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using Moq.Language.Flow;
using NUnit.Framework;
using RDeF.Entities;
using RDeF.Mapping;
using RDeF.Mapping.Entities;
using RDeF.Mapping.Explicit;
using RDeF.Mapping.Providers;

namespace Given_instance_of.EntityAwareMappingsRepository_class
{
    public abstract class EntityAwareMappingsRepositoryTest
    {
        protected Mock<DefaultMappingsRepository> MappingRepository { get; set; }

        protected Mock<IExplicitMappings> ExplicitMappings { get; set; }

        protected Mock<IEntityMapping> EntityMapping { get; set; }

        protected Mock<IPropertyMapping> PropertyMapping { get; set; }

        protected Mock<IEntity> OwningEntity { get; set; }

        protected Iri Iri { get; set; }

        protected Iri Class { get; set; }

        protected Iri Predicate { get; set; }

        protected EntityAwareMappingsRepository Repository { get; set; }

        [SetUp]
        public void Setup()
        {
            var context = new Mock<IEntityContext>(MockBehavior.Strict);
            Class = new Iri();
            Predicate = new Iri();
            EntityMapping = new Mock<IEntityMapping>(MockBehavior.Strict);
            PropertyMapping = new Mock<IPropertyMapping>(MockBehavior.Strict);
            var mappingBuilder = new Mock<IMappingBuilder>(MockBehavior.Strict);
            mappingBuilder.Setup(instance => instance.BuildMappings(It.IsAny<IEnumerable<IMappingSource>>(), It.IsAny<IDictionary<Type, ICollection<ITermMappingProvider>>>()))
                .Returns(new Dictionary<Type, IEntityMapping>());
            MappingRepository = new Mock<DefaultMappingsRepository>(MockBehavior.Strict, Array.Empty<IMappingSource>(), mappingBuilder.Object);
            ExplicitMappings = new Mock<IExplicitMappings>(MockBehavior.Strict);
            EntityContextExtensions.ExplicitMappings[context.Object] = ExplicitMappings.Object;
            Iri = new Iri("http://temp.uri/");
            OwningEntity = new Mock<IEntity>(MockBehavior.Strict);
            OwningEntity.SetupGet(instance => instance.Iri).Returns(Iri);
            ScenarioSetup();
            Repository = new EntityAwareMappingsRepository(() => context.Object, MappingRepository.Object);
        }

        [TearDown]
        public void Teardown()
        {
            EntityContextExtensions.ExplicitMappings.Clear();
        }

        internal TestSetup<TType> Configuring<TType>(Mock<TType> mock) where TType : class
        {
            return new TestSetup<TType>(this, mock);
        }

        protected virtual void ScenarioSetup()
        {
        }

        internal class TestSetup<TType> where TType : class
        {
            private readonly EntityAwareMappingsRepositoryTest _fixture;
            private readonly Mock<TType> _mock;

            internal TestSetup(EntityAwareMappingsRepositoryTest fixture, Mock<TType> mock)
            {
                _fixture = fixture;
                _mock = mock;
            }

            internal TestSetup<TType, TResult> Member<TResult>(Expression<Func<TType, TResult>> member)
            {
                return new TestSetup<TType, TResult>(_fixture, _mock, _mock.Setup(member), member);
            }
        }

        internal class TestSetup<TType, TResult> where TType : class
        {
            private readonly Mock<TType> _mock;
            private readonly Expression<Func<TType, TResult>> _member;
            private readonly ISetup<TType, TResult> _setup;
            private TResult _result;

            internal TestSetup(EntityAwareMappingsRepositoryTest fixture, Mock<TType> mock, ISetup<TType, TResult> setup, Expression<Func<TType, TResult>> member)
            {
                And = fixture;
                _mock = mock;
                _setup = setup;
                _member = member;
            }

            internal EntityAwareMappingsRepositoryTest And { get; }

            internal TestSetup<TType, TResult> ToReturn(TResult value)
            {
                _setup.Returns(value);
                return this;
            }

            internal TestSetup<TType, TResult> Calling(Func<EntityAwareMappingsRepository, TResult> action)
            {
                _result = action(And.Repository);
                return this;
            }

            internal TestSetup<TType, TResult> ShouldCallConfiguredMemberWith(params object[] parameters)
            {
                var methodCall = (MethodCallExpression)_member.Body;
                var correctMethodCall = Expression.Call(
                    _member.Parameters[0],
                    methodCall.Method,
                    parameters.Select((parameter, index) => Expression.Constant(parameter, methodCall.Method.GetParameters()[index].ParameterType)));
                _mock.Verify(Expression.Lambda<Func<TType, TResult>>(correctMethodCall, _member.Parameters[0]));
                return this;
            }

            internal void ResultingWithExpected(TResult result)
            {
                _result.Should().BeEquivalentTo(result);
            }
        }
    }
}
