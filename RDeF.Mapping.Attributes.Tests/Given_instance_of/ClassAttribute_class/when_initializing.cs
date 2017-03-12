﻿using System;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Mapping.Attributes;

namespace Given_instance_of.ClassAttribute_class
{
    [TestFixture]
    public class when_initializing
    {
        [Test]
        public void Should_throw_when_no_prefix_is_given()
        {
            ((ClassAttribute)null).Invoking(_ => new ClassAttribute(null, (string)null, (string)null))
                .ShouldThrow<ArgumentNullException>().Which.ParamName.Should().Be("prefix");
        }

        [Test]
        public void Should_throw_when_an_empty_prefix_is_given()
        {
            ((ClassAttribute)null).Invoking(_ => new ClassAttribute(String.Empty, (string)null, (string)null))
                .ShouldThrow<ArgumentOutOfRangeException>().Which.ParamName.Should().Be("prefix");
        }

        [Test]
        public void Should_throw_when_no_term_is_given()
        {
            ((ClassAttribute)null).Invoking(_ => new ClassAttribute("prefix", (string)null, (string)null))
                .ShouldThrow<ArgumentNullException>().Which.ParamName.Should().Be("term");
        }

        [Test]
        public void Should_throw_when_an_empty_term_is_given()
        {
            ((ClassAttribute)null).Invoking(_ => new ClassAttribute("prefix", String.Empty, (string)null))
                .ShouldThrow<ArgumentOutOfRangeException>().Which.ParamName.Should().Be("term");
        }

        [Test]
        public void Should_throw_when_no_Iri_given()
        {
            ((ClassAttribute)null).Invoking(_ => new ClassAttribute(null))
                .ShouldThrow<ArgumentNullException>().Which.ParamName.Should().Be("iri");
        }

        [Test]
        public void Should_throw_when_an_empty_Iri_is_given()
        {
            ((ClassAttribute)null).Invoking(_ => new ClassAttribute(String.Empty))
                .ShouldThrow<ArgumentOutOfRangeException>().Which.ParamName.Should().Be("iri");
        }
    }
}
