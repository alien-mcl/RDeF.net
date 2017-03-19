namespace RDeF.Mapping
{
    /// <summary>Defines possible collection RDF storage models.</summary>
    public enum CollectionStorageModel
    {
        /// <summary>Defines an unspecified storage model.</summary>
        Unspecified,

        /// <summary>Defines a simple storage model, where each item is an another statement with same predicate.</summary>
        Simple,

        /// <summary>Defines a linked list storage model, where each item maintains it's order.</summary>
        LinkedList
    }
}