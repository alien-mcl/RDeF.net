﻿using RDeF.Entities;

namespace RDeF.Mapping
{
    /// <summary>Represents a property mapping.</summary>
    public interface IPropertyMapping
    {
        /// <summary>Gets the entity mapping owning this property mapping.</summary>
        IEntityMapping EntityMapping { get; }

        /// <summary>Gets the name of the property being mapped.</summary>
        string Name { get; }

        /// <summary>Gets the optional graph requirement by the mapping.</summary>
        Iri Graph { get; }

        /// <summary>Gets the predicate being mapped.</summary>
        Iri Predicate { get; }

        /// <summary>Gets the value converter used by the mapping.</summary>
        IConverter ValueConverter { get; }
    }
}