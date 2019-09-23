﻿using System;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Entities;

namespace Given_instance_of.Iri_class
{
    [TestFixture]
    public class when_converting
    {
        private static readonly Uri Uri = new Uri("http://test.com/");

        [Test]
        public void Should_return_null_for_null_Iri_operand()
        {
            ((Uri)(Iri)null).Should().BeNull();
        }

        [Test]
        public void Should_return_null_for_null_Uri_operand()
        {
            ((Iri)(Uri)null).Should().BeNull();
        }

        [Test]
        public void Should_return_an_original_Uri()
        {
            ((Uri)new Iri(Uri)).Should().Be(Uri);
        }

        [Test]
        public void Should_return_a_Uri()
        {
            ((Uri)new Iri("some:test")).ToString().Should().Be("some:test");
        }

        [Test]
        public void Should_return_an_Iri()
        {
            ((Iri)Uri).ToString().Should().Be(Uri.ToString());
        }

        [Test]
        public void Should_return_an_original_string_passed_to_Iri()
        {
            ((string)new Iri("some:test")).Should().Be("some:test");
        }
    }
}
