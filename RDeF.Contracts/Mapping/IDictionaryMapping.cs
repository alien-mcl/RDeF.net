namespace RDeF.Mapping
{
    /// <summary>Describes a dictionary mapping.</summary>
    public interface IDictionaryMapping : IPropertyMapping
    {
        /// <summary>Gets the key converter.</summary>
        IConverter KeyConverter { get; }
    }
}
