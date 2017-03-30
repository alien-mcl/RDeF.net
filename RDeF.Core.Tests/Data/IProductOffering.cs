using System.Collections.Generic;
using RDeF.Mapping.Attributes;

namespace RDeF.Data
{
    [Class(Iri = "Offering")]
    public interface IProductOffering<T> : IService where T : IProduct
    {
        [Property(Iri = "oferredProduct")]
        T OfferedProduct { get; set; }

        [Collection(Iri = "texts")]
        ICollection<string> Texts { get; }
    }
}
