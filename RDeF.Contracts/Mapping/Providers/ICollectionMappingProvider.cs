namespace RDeF.Mapping.Providers
{
    /// <summary>Describes an abstract collection mapping provider.</summary>
    public interface ICollectionMappingProvider : IPropertyMappingProvider
    {
        /// <summary>Gets or sets the storage model.</summary>
        CollectionStorageModel StoreAs { get; set; }
    }
}
