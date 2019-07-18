#pragma warning disable SA1300 // Element must begin with upper-case letter
#pragma warning disable SA1307 // Accessible fields must begin with upper-case letter
#pragma warning disable SA1311 // Static readonly fields must begin with upper-case letter
#pragma warning disable SA1303 // Const field names must begin with upper-case letter
using System.Diagnostics.CodeAnalysis;
using RDeF.Entities;

namespace RDeF.Vocabularies
{
    /// <summary>Exposes RDF terms.</summary>
    [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with RDF namespace convention.")]
    public static class rdf
    {
        /// <summary>Defines RDF base url.</summary>
        public const string ns = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";

        /// <summary>Defines RDF base url.</summary>
        public static readonly Iri Namespace = new Iri(ns);

        /// <summary>Defines rdf:Property class.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with RDF namespace convention.")]
        public static readonly Iri Property = Namespace + "Property";

        /// <summary>Defines rdf:Statement class.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with RDF namespace convention.")]
        public static readonly Iri Statement = Namespace + "Statement";

        /// <summary>Defines rdf:List class.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with RDF namespace convention.")]
        public static readonly Iri List = Namespace + "List";

        /// <summary>Defines rdf:Bag class.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with RDF namespace convention.")]
        public static readonly Iri Bag = Namespace + "Bag";

        /// <summary>Defines rdf:Seq class.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with RDF namespace convention.")]
        public static readonly Iri Seq = Namespace + "Seq";

        /// <summary>Defines rdf:Alt class.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with RDF namespace convention.")]
        public static readonly Iri Alt = Namespace + "Alt";

        /// <summary>Defines rdf:first predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with RDF namespace convention.")]
        public static readonly Iri first = Namespace + "first";

        /// <summary>Defines rdf:rest predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with RDF namespace convention.")]
        public static readonly Iri rest = Namespace + "rest";

        /// <summary>Defines rdf:first predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with RDF namespace convention.")]
        public static readonly Iri nil = Namespace + "nil";

        /// <summary>Defines rdf:type predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with RDF namespace convention.")]
        public static readonly Iri type = Namespace + "type";

        /// <summary>Defines rdf:subject predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with RDF namespace convention.")]
        public static readonly Iri subject = Namespace + "subject";

        /// <summary>Defines rdf:predicate predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with RDF namespace convention.")]
        public static readonly Iri predicate = Namespace + "predicate";

        /// <summary>Defines rdf:object predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with RDF namespace convention.")]
        public static readonly Iri @object = Namespace + "object";

        /// <summary>Defines rdf:value predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with RDF namespace convention.")]
        public static readonly Iri value = Namespace + "value";

        /// <summary>Defines rdf:langString predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with RDF namespace convention.")]
        public static readonly Iri langString = Namespace + "langString";
    }
}
#pragma warning restore SA1303 // Const field names must begin with upper-case letter
#pragma warning restore SA1311 // Static readonly fields must begin with upper-case letter
#pragma warning restore SA1307 // Accessible fields must begin with upper-case letter
#pragma warning restore SA1300 // Element must begin with upper-case letter
