#pragma warning disable SA1300 // Element must begin with upper-case letter
#pragma warning disable SA1307 // Accessible fields must begin with upper-case letter
#pragma warning disable SA1311 // Static readonly fields must begin with upper-case letter
#pragma warning disable SA1303 // Const field names must begin with upper-case letter
using System.Diagnostics.CodeAnalysis;
using RDeF.Entities;

namespace RDeF.Vocabularies
{
    /// <summary>Exposes XSD terms.</summary>
    [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with XSD namespace convention.")]
    public static class xsd
    {
        /// <summary>Defines XSD base url.</summary>
        public const string ns = "http://www.w3.org/2001/XMLSchema#";

        /// <summary>Defines XSD base url.</summary>
        public static readonly Iri Namespace = new Iri(ns);

        /// <summary>Defines xsd:int predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with XSD namespace convention.")]
        public static readonly Iri @int = Namespace + "int";

        /// <summary>Defines xsd:float predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with XSD namespace convention.")]
        public static readonly Iri @float = Namespace + "float";

        /// <summary>Defines xsd:double predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with XSD namespace convention.")]
        public static readonly Iri @double = Namespace + "double";

        /// <summary>Defines xsd:decimal predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with XSD namespace convention.")]
        public static readonly Iri @decimal = Namespace + "decimal";

        /// <summary>Defines xsd:integer predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with XSD namespace convention.")]
        public static readonly Iri integer = Namespace + "integer";

        /// <summary>Defines xsd:long predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with XSD namespace convention.")]
        public static readonly Iri @long = Namespace + "long";

        /// <summary>Defines xsd:byte predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with XSD namespace convention.")]
        public static readonly Iri @byte = Namespace + "byte";

        /// <summary>Defines xsd:short predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with XSD namespace convention.")]
        public static readonly Iri @short = Namespace + "short";

        /// <summary>Defines xsd:dateTime predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with XSD namespace convention.")]
        public static readonly Iri dateTime = Namespace + "dateTime";

        /// <summary>Defines xsd:date predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with XSD namespace convention.")]
        public static readonly Iri date = Namespace + "date";

        /// <summary>Defines xsd:boolean predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with XSD namespace convention.")]
        public static readonly Iri boolean = Namespace + "boolean";

        /// <summary>Defines xsd:time predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with XSD namespace convention.")]
        public static readonly Iri time = Namespace + "time";

        /// <summary>Defines xsd:unsignedInt predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with XSD namespace convention.")]
        public static readonly Iri unsignedInt = Namespace + "unsignedInt";

        /// <summary>Defines xsd:unsignedByte predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with XSD namespace convention.")]
        public static readonly Iri unsignedByte = Namespace + "unsignedByte";

        /// <summary>Defines xsd:unsignedLong predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with XSD namespace convention.")]
        public static readonly Iri unsignedLong = Namespace + "unsignedLong";

        /// <summary>Defines xsd:unsignedShort predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with XSD namespace convention.")]
        public static readonly Iri unsignedShort = Namespace + "unsignedShort";

        /// <summary>Defines xsd:unsignedInteger predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with XSD namespace convention.")]
        public static readonly Iri unsignedInteger = Namespace + "unsignedInteger";

        /// <summary>Defines xsd:positiveInteger predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with XSD namespace convention.")]
        public static readonly Iri positiveInteger = Namespace + "positiveInteger";

        /// <summary>Defines xsd:nonNegativeInteger predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with XSD namespace convention.")]
        public static readonly Iri nonNegativeInteger = Namespace + "nonNegativeInteger";

        /// <summary>Defines xsd:nonPositiveInteger predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with XSD namespace convention.")]
        public static readonly Iri nonPositiveInteger = Namespace + "nonPositiveInteger";

        /// <summary>Defines xsd:duration predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with XSD namespace convention.")]
        public static readonly Iri duration = Namespace + "duration";

        /// <summary>Defines xsd:anyUri predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with XSD namespace convention.")]
        public static readonly Iri anyUri = Namespace + "anyUri";

        /// <summary>Defines xsd:string predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with XSD namespace convention.")]
        public static readonly Iri @string = Namespace + "string";

        /// <summary>Defines xsd:base64Binary predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with XSD namespace convention.")]
        public static readonly Iri base64Binary = Namespace + "base64Binary";

        /// <summary>Defines xsd:NCName predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with XSD namespace convention.")]
        public static readonly Iri NCName = Namespace + "NCName";
    }
}
#pragma warning restore SA1303 // Const field names must begin with upper-case letter
#pragma warning restore SA1311 // Static readonly fields must begin with upper-case letter
#pragma warning restore SA1307 // Accessible fields must begin with upper-case letter
#pragma warning restore SA1300 // Element must begin with upper-case letter
