#pragma warning disable SA1300 // Element must begin with upper-case letter
#pragma warning disable SA1307 // Accessible fields must begin with upper-case letter
#pragma warning disable SA1311 // Static readonly fields must begin with upper-case letter
#pragma warning disable SA1303 // Const field names must begin with upper-case letter
using System.Diagnostics.CodeAnalysis;
using RDeF.Entities;

namespace RDeF.Vocabularies
{
    /// <summary>Exposes OWL terms.</summary>
    [ExcludeFromCodeCoverage]
    [SuppressMessage("UnitTests", "TS0000:NoUnitTests", Justification = "No testable logic.")]
    [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
    public static class owl
    {
        /// <summary>Defines OWL base url.</summary>
        public const string ns = "http://www.w3.org/2002/07/owl#";

        /// <summary>Defines OWL base url.</summary>
        public static readonly Iri Namespace = new Iri(ns);

        /// <summary>Defines owl:AllDifferent class.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri AllDifferent = Namespace + "AllDifferent";

        /// <summary>Defines owl:AllDisjointClasses class.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri AllDisjointClasses = Namespace + "AllDisjointClasses";

        /// <summary>Defines owl:AllDisjointProperties class.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri AllDisjointProperties = Namespace + "AllDisjointProperties";

        /// <summary>Defines owl:Annotation class.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri Annotation = Namespace + "Annotation";

        /// <summary>Defines owl:AnnotationProperty class.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri AnnotationProperty = Namespace + "AnnotationProperty";

        /// <summary>Defines owl:AsymmetricProperty class.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri AsymmetricProperty = Namespace + "AsymmetricProperty";

        /// <summary>Defines owl:Axiom class.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri Axiom = Namespace + "Axiom";

        /// <summary>Defines owl:Class class.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri Class = Namespace + "Class";

        /// <summary>Defines owl:DataRange class.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri DataRange = Namespace + "DataRange";

        /// <summary>Defines owl:DatatypeProperty class.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri DatatypeProperty = Namespace + "DatatypeProperty";

        /// <summary>Defines owl:DeprecatedClass class.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri DeprecatedClass = Namespace + "DeprecatedClass";

        /// <summary>Defines owl:DeprecatedProperty class.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri DeprecatedProperty = Namespace + "DeprecatedProperty";

        /// <summary>Defines owl:FunctionalProperty class.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri FunctionalProperty = Namespace + "FunctionalProperty";

        /// <summary>Defines owl:InverseFunctionalProperty class.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri InverseFunctionalProperty = Namespace + "InverseFunctionalProperty";

        /// <summary>Defines owl:IrreflexiveProperty class.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri IrreflexiveProperty = Namespace + "IrreflexiveProperty";

        /// <summary>Defines owl:NamedIndividual class.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri NamedIndividual = Namespace + "NamedIndividual";

        /// <summary>Defines owl:NegativePropertyAssertion class.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri NegativePropertyAssertion = Namespace + "NegativePropertyAssertion";

        /// <summary>Defines owl:ObjectProperty class.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri ObjectProperty = Namespace + "ObjectProperty";

        /// <summary>Defines owl:Ontology class.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri Ontology = Namespace + "Ontology";

        /// <summary>Defines owl:OntologyProperty class.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri OntologyProperty = Namespace + "OntologyProperty";

        /// <summary>Defines owl:ReflexiveProperty class.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri ReflexiveProperty = Namespace + "ReflexiveProperty";

        /// <summary>Defines owl:Restriction class.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri Restriction = Namespace + "Restriction";

        /// <summary>Defines owl:SymmetricProperty class.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri SymmetricProperty = Namespace + "SymmetricProperty";

        /// <summary>Defines owl:TransitiveProperty class.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri TransitiveProperty = Namespace + "TransitiveProperty";

        /// <summary>Defines owl:Nothing class.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri Nothing = Namespace + "Nothing";

        /// <summary>Defines owl:Thing class.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri Thing = Namespace + "Thing";

        /// <summary>Defines owl:allValuesFrom predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri allValuesFrom = Namespace + "allValuesFrom";

        /// <summary>Defines owl:annotatedProperty predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri annotatedProperty = Namespace + "annotatedProperty";

        /// <summary>Defines owl:annotatedSource predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri annotatedSource = Namespace + "annotatedSource";

        /// <summary>Defines owl:annotatedTarget predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri annotatedTarget = Namespace + "annotatedTarget";

        /// <summary>Defines owl:assertionProperty predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri assertionProperty = Namespace + "assertionProperty";

        /// <summary>Defines owl:cardinality predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri cardinality = Namespace + "cardinality";

        /// <summary>Defines owl:complementOf predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri complementOf = Namespace + "complementOf";

        /// <summary>Defines owl:datatypeComplementOf predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri datatypeComplementOf = Namespace + "datatypeComplementOf";

        /// <summary>Defines owl:differentFrom predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri differentFrom = Namespace + "differentFrom";

        /// <summary>Defines owl:disjointUnionOf predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri disjointUnionOf = Namespace + "disjointUnionOf";

        /// <summary>Defines owl:disjointWith predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri disjointWith = Namespace + "disjointWith";

        /// <summary>Defines owl:distinctMembers predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri distinctMembers = Namespace + "distinctMembers";

        /// <summary>Defines owl:equivalentClass predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri equivalentClass = Namespace + "equivalentClass";

        /// <summary>Defines owl:equivalentProperty predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri equivalentProperty = Namespace + "equivalentProperty";

        /// <summary>Defines owl:hasKey predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri hasKey = Namespace + "hasKey";

        /// <summary>Defines owl:hasSelf predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri hasSelf = Namespace + "hasSelf";

        /// <summary>Defines owl:hasValue predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri hasValue = Namespace + "hasValue";

        /// <summary>Defines owl:intersectionOf predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri intersectionOf = Namespace + "intersectionOf";

        /// <summary>Defines owl:inverseOf predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri inverseOf = Namespace + "inverseOf";

        /// <summary>Defines owl:maxCardinality predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri maxCardinality = Namespace + "maxCardinality";

        /// <summary>Defines owl:maxQualifiedCardinality predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri maxQualifiedCardinality = Namespace + "maxQualifiedCardinality";

        /// <summary>Defines owl:members predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri members = Namespace + "members";

        /// <summary>Defines owl:minCardinality predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri minCardinality = Namespace + "minCardinality";

        /// <summary>Defines owl:minQualifiedCardinality predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri minQualifiedCardinality = Namespace + "minQualifiedCardinality";

        /// <summary>Defines owl:onClass predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri onClass = Namespace + "onClass";

        /// <summary>Defines owl:onDataRange predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri onDataRange = Namespace + "onDataRange";

        /// <summary>Defines owl:onDatatype predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri onDatatype = Namespace + "onDatatype";

        /// <summary>Defines owl:oneOf predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri oneOf = Namespace + "oneOf";

        /// <summary>Defines owl:onProperties predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri onProperties = Namespace + "onProperties";

        /// <summary>Defines owl:onProperty predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri onProperty = Namespace + "onProperty";

        /// <summary>Defines owl:propertyChainAxiom predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri propertyChainAxiom = Namespace + "propertyChainAxiom";

        /// <summary>Defines owl:propertyDisjointWith predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri propertyDisjointWith = Namespace + "propertyDisjointWith";

        /// <summary>Defines owl:qualifiedCardinality predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri qualifiedCardinality = Namespace + "qualifiedCardinality";

        /// <summary>Defines owl:sameAs predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri sameAs = Namespace + "sameAs";

        /// <summary>Defines owl:someValuesFrom predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri someValuesFrom = Namespace + "someValuesFrom";

        /// <summary>Defines owl:sourceIndividual predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri sourceIndividual = Namespace + "sourceIndividual";

        /// <summary>Defines owl:targetIndividual predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri targetIndividual = Namespace + "targetIndividual";

        /// <summary>Defines owl:targetValue predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri targetValue = Namespace + "targetValue";

        /// <summary>Defines owl:unionOf predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri unionOf = Namespace + "unionOf";

        /// <summary>Defines owl:withRestrictions predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri withRestrictions = Namespace + "withRestrictions";

        /// <summary>Defines owl:bottomDataProperty predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri bottomDataProperty = Namespace + "bottomDataProperty";

        /// <summary>Defines owl:topDataProperty predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri topDataProperty = Namespace + "topDataProperty";

        /// <summary>Defines owl:bottomObjectProperty predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri bottomObjectProperty = Namespace + "bottomObjectProperty";

        /// <summary>Defines owl:topObjectProperty predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri topObjectProperty = Namespace + "topObjectProperty";

        /// <summary>Defines owl:backwardCompatibleWith predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri backwardCompatibleWith = Namespace + "backwardCompatibleWith";

        /// <summary>Defines owl:deprecated predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri deprecated = Namespace + "deprecated";

        /// <summary>Defines owl:incompatibleWith predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri incompatibleWith = Namespace + "incompatibleWith";

        /// <summary>Defines owl:priorVersion predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri priorVersion = Namespace + "priorVersion";

        /// <summary>Defines owl:versionInfo predicate.</summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "This is due to compliance with OWL namespace convention.")]
        public static readonly Iri versionInfo = Namespace + "versionInfo";
    }
}
#pragma warning restore SA1303 // Const field names must begin with upper-case letter
#pragma warning restore SA1311 // Static readonly fields must begin with upper-case letter
#pragma warning restore SA1307 // Accessible fields must begin with upper-case letter
#pragma warning restore SA1300 // Element must begin with upper-case letter
