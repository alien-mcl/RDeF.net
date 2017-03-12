namespace RDeF.Mapping
{
    /// <summary>Describes an abstract collection mapping.</summary>
    public interface ICollectionMapping : IPropertyMapping
    {
        /// <summary>Gets the storage model.</summary>
        CollectionStorageModel StoreAs { get; }
    }
}
