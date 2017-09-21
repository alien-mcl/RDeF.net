using NUnit.Framework;
using RDeF.Data;
using RDeF.Entities;
using RDeF.Mapping.Reflection;

namespace Given_instance_of.property_of_type
{
    [TestFixture]
    public class ExplicitlyMappedPropertyInfo_class : PropertyInfoTest<ExplicitlyMappedPropertyInfo>
    {
        protected override ExplicitlyMappedPropertyInfo PropertyInfo { get; } =
            new ExplicitlyMappedPropertyInfo(typeof(IUnmappedProduct).GetProperty("Name"), new Iri("predicate"), new Iri("graph"));
    }
}
