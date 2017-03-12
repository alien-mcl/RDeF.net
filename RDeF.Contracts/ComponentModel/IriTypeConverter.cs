using System;
using System.ComponentModel;
using System.Globalization;
using RDeF.Entities;

namespace RDeF.ComponentModel
{
    /// <summary>Converts from and to <see cref="Iri" /> either <see cref="string" /> or <see cref="Uri" /> values.</summary>
    public class IriTypeConverter : TypeConverter
    {
        /// <inheritdoc />
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return (sourceType == typeof(string)) || (sourceType == typeof(Uri)) || (base.CanConvertFrom(context, sourceType));
        }

        /// <inheritdoc />
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return (destinationType == typeof(string)) || (destinationType == typeof(Uri)) || (base.CanConvertTo(context, destinationType));
        }

        /// <inheritdoc />
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value == null)
            {
                return null;
            }

            var stringValue = value as string;
            if (stringValue != null)
            {
                return (stringValue.Length == 0 ? null : new Iri(stringValue));
            }

            var uriValue = value as Uri;
            return (uriValue != null ? new Iri(uriValue) : base.ConvertFrom(context, culture, value));
        }

        /// <inheritdoc />
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value == null)
            {
                return null;
            }

            if (destinationType == typeof(string))
            {
                return value.ToString();
            }

            if (destinationType == typeof(Uri))
            {
                Iri iriValue = value as Iri;
                if (iriValue == null)
                {
                    return null;
                }

                if (iriValue.Uri != null)
                {
                    return iriValue.Uri;
                }

                return new Uri(value.ToString(), UriKind.RelativeOrAbsolute);
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
