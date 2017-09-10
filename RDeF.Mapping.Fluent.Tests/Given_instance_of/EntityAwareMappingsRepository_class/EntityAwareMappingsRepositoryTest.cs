using System;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using Moq.Language.Flow;
using NUnit.Framework;
using RDeF.Entities;
using RDeF.Mapping;
using RDeF.Mapping.Explicit;

namespace Given_instance_of.EntityAwareMappingsRepository_class
{
    public abstract class EntityAwareMappingsRepositoryTest
    {
        protected Mock<IMappingsRepository> MappingRepository { get; set; }

        protected Mock<IExplicitMappings> ExplicitMappings { get; set; }

        protected Mock<IEntityMapping> EntityMapping { get; set; }

        protected Mock<IPropertyMapping> PropertyMapping { get; set; }

        protected Iri OwningEntity { get; set; }

        protected Iri Class { get; set; }

        protected Iri Predicate { get; set; }

        protected EntityAwareMappingsRepository Repository { get; set; }

        [SetUp]
        public void Setup()
        {
            Class = new Iri();
            Predicate = new Iri();
            EntityMapping = new Mock<IEntityMapping>(MockBehavior.Strict);
            PropertyMapping = new Mock<IPropertyMapping>(MockBehavior.Strict);
            MappingRepository = new Mock<IMappingsRepository>(MockBehavior.Strict);
            ExplicitMappings = new Mock<IExplicitMappings>(MockBehavior.Strict);
            OwningEntity = new Iri("http://temp.uri/");
            ScenarioSetup();
            Repository = new EntityAwareMappingsRepository(MappingRepository.Object, ExplicitMappings.Object, OwningEntity);
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
            private readonly EntityAwareMappingsRepositoryTest _fixture;
            private readonly Mock<TType> _mock;
            private readonly Expression<Func<TType, TResult>> _member;
            private readonly ISetup<TType, TResult> _setup;
            private TResult _result;

            internal TestSetup(EntityAwareMappingsRepositoryTest fixture, Mock<TType> mock, ISetup<TType, TResult> setup, Expression<Func<TType, TResult>> member)
            {
                _fixture = fixture;
                _mock = mock;
                _setup = setup;
                _member = member;
            }

            internal EntityAwareMappingsRepositoryTest And
            {
                get { return _fixture; }
            }

            internal TestSetup<TType, TResult> ToReturn(TResult value)
            {
                _setup.Returns(value);
                return this;
            }

            internal TestSetup<TType, TResult> Calling(Func<EntityAwareMappingsRepository, TResult> action)
            {
                _result = action(_fixture.Repository);
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
                _result.Should().Be(result);
            }
        }
    }
}
