using System;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Data;
using RDeF.Mapping.Reflection;
using RollerCaster;

namespace Given_instance_of.DynamicPropertyInfo_class
{
    [TestFixture]
    public class when_working_with_it
    {
        private DynamicPropertyInfo PropertyInfo { get; set; }

        private IUnmappedProduct Entity { get; set; }

        [Test]
        public void Should_get_no_attributes()
        {
            PropertyInfo.Attributes.Should().Be(PropertyAttributes.None);
        }

        [Test]
        public void Should_be_readable()
        {
            PropertyInfo.CanRead.Should().BeTrue();
        }

        [Test]
        public void Should_be_writable()
        {
            PropertyInfo.CanWrite.Should().BeTrue();
        }

        [Test]
        public void Should_have_same_reflected_and_declaring_type()
        {
            PropertyInfo.ReflectedType.Should().Be(PropertyInfo.DeclaringType);
        }

        [Test]
        public void Should_be_of_correct_type()
        {
            PropertyInfo.PropertyType.Should().Be(typeof(string));
        }

        [Test]
        public void Should_be_of_correct_name()
        {
            PropertyInfo.Name.Should().Be("Name");
        }

        [Test]
        public void Should_get_accessors()
        {
            PropertyInfo.GetAccessors().Should().HaveCount(2);
        }

        [Test]
        public void Should_get_getter()
        {
            PropertyInfo.GetGetMethod().Should().BeAssignableTo<MethodInfo>();
        }

        [Test]
        public void Should_get_setter()
        {
            PropertyInfo.GetSetMethod().Should().BeAssignableTo<MethodInfo>();
        }

        [Test]
        public void Should_get_no_index_parameters()
        {
            PropertyInfo.GetIndexParameters().Should().BeEmpty();
        }

        [Test]
        public void Should_get_no_custom_attributes()
        {
            PropertyInfo.GetCustomAttributes(true).Should().BeEquivalentTo(PropertyInfo.GetCustomAttributes(typeof(Attribute), true))
                .And.Subject.Should().BeEmpty();
        }

        [Test]
        public void Should_define_no_attribute()
        {
            PropertyInfo.IsDefined(typeof(Attribute), true).Should().BeFalse();
        }

        [Test]
        public void Should_get_value()
        {
            PropertyInfo.GetAccessors().First().Invoke(PropertyInfo, new object[] { Entity }).Should().Be("test");
        }

        [SetUp]
        public void Setup()
        {
            PropertyInfo = new DynamicPropertyInfo(typeof(IUnmappedProduct), typeof(string), "Name");
            var entity = new MulticastObject();
            entity.SetProperty(PropertyInfo, "test");
            Entity = entity.ActLike<IUnmappedProduct>();
            PropertyInfo.GetAccessors().Last().Invoke(PropertyInfo, new object[] { Entity, "test", null });
        }
    }
}
