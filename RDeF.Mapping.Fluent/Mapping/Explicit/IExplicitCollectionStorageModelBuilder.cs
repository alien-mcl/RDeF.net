using RDeF.Entities;

namespace RDeF.Mapping.Explicit
{
    /// <summary>Describes an abstract explicitely defined collection storage model builder.</summary>
    /// <typeparam name="TEntity">Type of the entity being mapped.</typeparam>
    public interface IExplicitCollectionStorageModelBuilder<TEntity> where TEntity : IEntity
    {
        /// <summary>Allows to map current collection in scope to a given storage model.</summary>
        /// <param name="storeAs">Storage model to be used.</param>
        /// <returns>Instance of the <see cref="IExplicitValueConverterBuilder{TEntity}" /> that will allow to configure a value converter.</returns>
        IExplicitValueConverterBuilder<TEntity> StoredAs(CollectionStorageModel storeAs);
    }
}
