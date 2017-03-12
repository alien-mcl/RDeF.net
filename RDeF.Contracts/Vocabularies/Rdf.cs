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

        /// <summary>Defines rdf:first predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with RDF namespace convention.")]
        public static readonly Iri first = Namespace + "first";

        /// <summary>Defines rdf:first predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with RDF namespace convention.")]
        public static readonly Iri last = Namespace + "last";

        /// <summary>Defines rdf:first predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with RDF namespace convention.")]
        public static readonly Iri nil = Namespace + "nil";
    }
}
#pragma warning restore SA1303 // Const field names must begin with upper-case letter
#pragma warning restore SA1311 // Static readonly fields must begin with upper-case letter
#pragma warning restore SA1307 // Accessible fields must begin with upper-case letter
#pragma warning restore SA1300 // Element must begin with upper-case letter
