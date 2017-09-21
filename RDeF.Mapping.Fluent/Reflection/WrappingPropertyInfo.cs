using System;
using System.Globalization;
using System.Reflection;
using RollerCaster;

namespace RDeF.Mapping.Reflection
{
    /// <summary>Describes a base functionality of a wrapping property info.</summary>
    public abstract class WrappingPropertyInfo : PropertyInfo
    {
        private readonly Func<object, object> _getter;
        private readonly Action<object, object, object[]> _setter;

        internal WrappingPropertyInfo(PropertyInfo wrappedPropertyInfo)
        {
            WrappedPropertyInfo = wrappedPropertyInfo;
            _getter = obj => GetValue(obj, BindingFlags.Instance, null, null, CultureInfo.InvariantCulture);
            _setter = (obj, value, index) => SetValue(obj, value, index);
        }

        /// <inheritdoc />
        public override Type PropertyType { get { return WrappedPropertyInfo.PropertyType; } }

        /// <inheritdoc />
        public override PropertyAttributes Attributes { get { return WrappedPropertyInfo.Attributes; } }

        /// <inheritdoc />
        public override bool CanRead { get { return WrappedPropertyInfo.CanRead; } }

        /// <inheritdoc />
        public override bool CanWrite { get { return WrappedPropertyInfo.CanWrite; } }

        /// <inheritdoc />
        public override string Name { get { return WrappedPropertyInfo.Name; } }

        /// <inheritdoc />
        public override Type DeclaringType { get { return WrappedPropertyInfo.DeclaringType; } }

        /// <inheritdoc />
        public override Type ReflectedType { get { return WrappedPropertyInfo.ReflectedType; } }

        internal PropertyInfo WrappedPropertyInfo { get; private set; }

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
            return WrappedPropertyInfo.GetIndexParameters();
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
            return WrappedPropertyInfo.GetCustomAttributes(inherit);
        }

        /// <inheritdoc />
        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return WrappedPropertyInfo.GetCustomAttributes(attributeType, inherit);
        }

        /// <inheritdoc />
        public override bool IsDefined(Type attributeType, bool inherit)
        {
            return WrappedPropertyInfo.IsDefined(attributeType, inherit);
        }
    }
}
