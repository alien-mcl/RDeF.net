namespace RDeF.Entities
{
    /// <summary>Describes a queryable <see cref="IEntitySource" />.</summary>
    internal interface IInMemoryEntitySource : IEntitySource, ISerializableEntitySource
    {
        /// <summary>Loads an entity of a given <paramref name="iri" />.</summary>
        /// <param name="iri">Iri of the entity to be loaded.</param>
        /// <returns>Entity loaded.</returns>
        new IEntity Create(Iri iri);

        /// <summary>Loads an entity of a given <paramref name="iri" />.</summary>
        /// <typeparam name="TEntity">Type of the entity to load.</typeparam>
        /// <param name="iri">Iri of the entity to be loaded.</param>
        /// <returns>Entity loaded.</returns>
        new TEntity Create<TEntity>(Iri iri) where TEntity : IEntity;
    }
}
