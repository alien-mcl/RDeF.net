using System;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Serialization;
using RDeF.Testing;

namespace Given_instance_of.RdfXmlReader_class
{
    [TestFixture]
    public class when_reading : RdfReaderTest<RdfXmlReader>
    {
        [Test]
        public void Should_throw_when_null_is_given_instead_of_stream_reader()
        {
            Reader.Awaiting(instance => instance.Read(null))
                .ShouldThrow<ArgumentNullException>().Which.ParamName.Should().Be("streamReader");
        }
    }
}
