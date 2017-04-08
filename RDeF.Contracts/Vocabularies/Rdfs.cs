#pragma warning disable SA1300 // Element must begin with upper-case letter
#pragma warning disable SA1307 // Accessible fields must begin with upper-case letter
#pragma warning disable SA1311 // Static readonly fields must begin with upper-case letter
#pragma warning disable SA1303 // Const field names must begin with upper-case letter
using System.Diagnostics.CodeAnalysis;
using RDeF.Entities;

namespace RDeF.Vocabularies
{
    /// <summary>Exposes RDFS terms.</summary>
    [ExcludeFromCodeCoverage]
    [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "No testable logic.")]
    [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with RDFS namespace convention.")]
    public static class rdfs
    {
        /// <summary>Defines RDFS base url.</summary>
        public const string ns = "http://www.w3.org/2000/01/rdf-schema#";

        /// <summary>Defines RDFS base url.</summary>
        public static readonly Iri Namespace = new Iri(ns);

        /// <summary>Defines rdfs:Resource class.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with RDFS namespace convention.")]
        public static readonly Iri Resource = Namespace + "Resource";

        /// <summary>Defines rdfs:Class class.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with RDFS namespace convention.")]
        public static readonly Iri Class = Namespace + "Class";

        /// <summary>Defines rdfs:Literal class.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with RDFS namespace convention.")]
        public static readonly Iri Literal = Namespace + "Literal";

        /// <summary>Defines rdfs:Container class.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with RDFS namespace convention.")]
        public static readonly Iri Container = Namespace + "Container";

        /// <summary>Defines rdfs:ContainerMembershipProperty class.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with RDFS namespace convention.")]
        public static readonly Iri ContainerMembershipProperty = Namespace + "ContainerMembershipProperty";

        /// <summary>Defines rdfs:Datatype class.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", Justification = "This is due to compliance with RDFS namespace convention.")]
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with RDFS namespace convention.")]
        public static readonly Iri Datatype = Namespace + "Datatype";

        /// <summary>Defines rdfs:subClassOf predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with RDFS namespace convention.")]
        public static readonly Iri subClassOf = Namespace + "subClassOf";

        /// <summary>Defines rdfs:subPropertyOf predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with RDFS namespace convention.")]
        public static readonly Iri subPropertyOf = Namespace + "subPropertyOf";

        /// <summary>Defines rdfs:comment predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with RDFS namespace convention.")]
        public static readonly Iri comment = Namespace + "comment";

        /// <summary>Defines rdfs:label predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with RDFS namespace convention.")]
        public static readonly Iri label = Namespace + "label";

        /// <summary>Defines rdfs:domain predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with RDFS namespace convention.")]
        public static readonly Iri domain = Namespace + "domain";

        /// <summary>Defines rdfs:range predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with RDFS namespace convention.")]
        public static readonly Iri range = Namespace + "range";

        /// <summary>Defines rdfs:seeAlso predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with RDFS namespace convention.")]
        public static readonly Iri seeAlso = Namespace + "seeAlso";

        /// <summary>Defines rdfs:isDefinedBy predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with RDFS namespace convention.")]
        public static readonly Iri isDefinedBy = Namespace + "isDefinedBy";

        /// <summary>Defines rdfs:member predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with RDFS namespace convention.")]
        public static readonly Iri member = Namespace + "member";
    }
}
#pragma warning restore SA1303 // Const field names must begin with upper-case letter
#pragma warning restore SA1311 // Static readonly fields must begin with upper-case letter
#pragma warning restore SA1307 // Accessible fields must begin with upper-case letter
#pragma warning restore SA1300 // Element must begin with upper-case letter
