﻿using System;
using System.Reflection;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Mapping.Explicit;

namespace Given_instance_of.DefaultExplicitPropertyMappingBuilder_class
{
    [TestFixture]
    public class when_building_a_property
    {
        private DefaultExplicitPropertyMappingBuilder<IUnmappedProduct> Builder { get; set; }

        [Test]
        public void Should_throw_when_no_prefix_is_given()
        {
            Builder.Invoking(instance => instance.MappedTo((string)null, null))
                .Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("prefix");
        }

        [Test]
        public void Should_throw_when_a_given_prefix_is_empty()
        {
            Builder.Invoking(instance => instance.MappedTo(String.Empty, null))
                .Should().Throw<ArgumentOutOfRangeException>().Which.ParamName.Should().Be("prefix");
        }

        [Test]
        public void Should_throw_when_no_term_is_given()
        {
            Builder.Invoking(instance => instance.MappedTo("test", null))
                .Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("term");
        }

        [Test]
        public void Should_throw_when_a_given_term_is_empty()
        {
            Builder.Invoking(instance => instance.MappedTo("test", String.Empty))
                .Should().Throw<ArgumentOutOfRangeException>().Which.ParamName.Should().Be("term");
        }

        [SetUp]
        public void Setup()
        {
            Builder = new DefaultExplicitPropertyMappingBuilder<IUnmappedProduct>(
                new DefaultExplicitMappingsBuilder<IUnmappedProduct>(),
                typeof(IUnmappedProduct).GetTypeInfo().GetProperty("Name"));
        }
    }
}
