﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Entities;
using RDeF.Mapping;
using RollerCaster;

namespace Given_instance_of.DefaultEntityContext_class
{
    [TestFixture]
    public class when_copying_entity : DefaultEntityContextTest
    {
        private static readonly Iri ExpectedIri = new Iri("copy");

        private IEntityContext SourceContext { get; set; }

        private IProduct Source { get; set; }

        private IProduct Result { get; set; }

        public override Task TheTest()
        {
            Result = Context.Copy(Source, ExpectedIri);
            return Task.CompletedTask;
        }

        [Test]
        public void Should_throw_when_no_entity_is_given()
        {
            Context.Invoking(instance => instance.Copy((IProduct)null))
                .Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("entity");
        }

        [Test]
        public void Should_return_same_entity_if_the_source_context_is_also_the_same_as_the_target_one()
        {
            Context.Copy(Context.Create<IProduct>(new Iri("some_iri"))).Iri.Should().Be(new Iri("some_iri"));
        }

        [Test]
        public void Should_throw_when_instance_given_is_not_entity()
        {
            Context.Invoking(instance => instance.Copy(new MulticastObject().ActLike<IProduct>()))
                .Should().Throw<ArgumentOutOfRangeException>().Which.ParamName.Should().Be("entity");
        }

        [Test]
        public void Should_assign_new_Iri()
        {
            Result.Iri.Should().Be(ExpectedIri);
        }

        [Test]
        public void Should_copy_that_entity_into_new_context()
        {
            Result.Context.Should().Be(Context);
        }

        [Test]
        public void Should_leave_source_entity_untouched()
        {
            Source.Context.Should().Be(SourceContext);
        }

        [Test]
        public void Should_copy_description()
        {
            Result.Description.Should().Be(Source.Description);
        }

        [Test]
        public void Should_copy_name()
        {
            Result.Name.Should().Be(Source.Name);
        }

        [Test]
        public void Should_copy_ordinal()
        {
            Result.Ordinal.Should().Be(Source.Ordinal);
        }

        [Test]
        public void Should_copy_price()
        {
            Result.Price.Should().Be(Source.Price);
        }

        [Test]
        public void Should_copy_comments()
        {
            Result.Comments.Should().BeEquivalentTo(Source.Comments);
        }

        [Test]
        public void Should_copy_categories()
        {
            Result.Categories.Should().BeEquivalentTo(Source.Categories);
        }

        protected override void ScenarioSetup()
        {
            SourceContext = new DefaultEntityContext(EntitySource.Object, MappingsRepository.Object, ChangeDetector.Object, Array.Empty<ILiteralConverter>());
            Source = SourceContext.Create<IProduct>(new Iri("source"));
            EntitySource.Setup(instance => instance.Load(It.IsAny<Iri>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IEnumerable<Statement>>(Array.Empty<Statement>()));
            MappingsRepository.Setup(instance => instance.FindPropertyMappingFor(It.IsAny<IEntity>(), It.IsAny<PropertyInfo>()))
                .Returns<IEntity, PropertyInfo>((entity, propertyInfo) =>
                {
                    var result = new Mock<IPropertyMapping>(MockBehavior.Strict);
                    result.SetupGet(instance => instance.PropertyInfo).Returns(propertyInfo);
                    return result.Object;
                });
        }
    }
}
