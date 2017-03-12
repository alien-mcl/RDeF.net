using System.ComponentModel;
using NUnit.Framework;
using RDeF.Entities;

namespace Given_instance_of.IriTypeConverter_class
{
    public abstract class IriTypeConverterTest
    {
        protected TypeConverter Converter { get; private set; }

        [SetUp]
        public void Setup()
        {
            Converter = TypeDescriptor.GetConverter(typeof(Iri));
        }
    }
}
