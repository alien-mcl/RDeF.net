using System;

namespace RDeF.Mapping
{
    /// <summary>Represents a property mapping.</summary>
    public interface IPropertyMapping : IStatementMapping
    {
        /// <summary>Gets the entity mapping owning this property mapping.</summary>
        IEntityMapping EntityMapping { get; }

        /// <summary>Gets the name of the property being mapped.</summary>
        string Name { get; }

        /// <summary>Gets a return type of the property.</summary>
        Type ReturnType { get; }

        /// <summary>Gets the value converter used by the mapping.</summary>
        IConverter ValueConverter { get; }
    }
}