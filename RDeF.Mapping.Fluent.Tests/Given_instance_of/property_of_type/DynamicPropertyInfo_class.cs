using NUnit.Framework;
using RDeF.Data;
using RDeF.Mapping.Reflection;

namespace Given_instance_of.property_of_type
{
    [TestFixture]
    public class DynamicPropertyInfo_class : PropertyInfoTest<DynamicPropertyInfo>
    {
        protected override DynamicPropertyInfo PropertyInfo { get; } = new DynamicPropertyInfo(typeof(IUnmappedProduct), typeof(string), "Name");
    }
}
