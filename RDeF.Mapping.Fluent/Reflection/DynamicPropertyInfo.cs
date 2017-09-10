using System;
using System.Globalization;
using System.Reflection;
using RollerCaster;
using RollerCaster.Reflection;

namespace RDeF.Mapping.Reflection
{
    /// <summary>Describes a virtual property to be used in dynamic context.</summary>
    public class DynamicPropertyInfo : PropertyInfo
    {
        private readonly Type _entityType;
        private readonly Type _propertyType;
        private readonly string _name;
        private readonly PropertyAttributes _attributes;
        private readonly Func<object, object> _getter;
        private readonly Action<object, object, object[]> _setter;

        internal DynamicPropertyInfo(Type entityType, Type propertyType, string name)
        {
            _entityType = entityType;
            _propertyType = propertyType;
            _name = name;
            _attributes = PropertyAttributes.None;
            _getter = obj => GetValue(obj, BindingFlags.Instance, null, null, CultureInfo.InvariantCulture);
            _setter = (obj, value, index) => SetValue(obj, value, index);
        }

        /// <inheritdoc />
        public override Type PropertyType { get { return _propertyType; } }

        /// <inheritdoc />
        public override PropertyAttributes Attributes { get { return _attributes; } }

        /// <inheritdoc />
        public override bool CanRead { get { return true; } }

        /// <inheritdoc />
        public override bool CanWrite { get { return !_propertyType.IsAnEnumerable(); } }

        /// <inheritdoc />
        public override string Name { get { return _name; } }

        /// <inheritdoc />
        public override Type DeclaringType { get { return _entityType; } }

        /// <inheritdoc />
        public override Type ReflectedType { get { return _entityType; } }

        /// <inheritdoc />
        public override MethodInfo[] GetAccessors(bool nonPublic)
        {
            return new[] { _getter.GetMethodInfo(), _setter.GetMethodInfo() };
        }

        /// <inheritdoc />
        public override MethodInfo GetGetMethod(bool nonPublic)
        {
            return _getter.GetMethodInfo();
        }

        /// <inheritdoc />
        public override MethodInfo GetSetMethod(bool nonPublic)
        {
            return _setter.GetMethodInfo();
        }

        /// <inheritdoc />
        public override ParameterInfo[] GetIndexParameters()
        {
            return Array.Empty<ParameterInfo>();
        }

        /// <inheritdoc />
        public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
        {
            return obj.Unwrap().GetProperty(this);
        }

        /// <inheritdoc />
        public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
        {
            obj.Unwrap().SetProperty(this, value);
        }

        /// <inheritdoc />
        public override object[] GetCustomAttributes(bool inherit)
        {
            return Array.Empty<Attribute>();
        }

        /// <inheritdoc />
        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return Array.Empty<Attribute>();
        }

        /// <inheritdoc />
        public override bool IsDefined(Type attributeType, bool inherit)
        {
            return false;
        }
    }
}
