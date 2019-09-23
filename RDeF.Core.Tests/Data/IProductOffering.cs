using System.Collections.Generic;
using RDeF.Mapping.Attributes;

namespace RDeF.Data
{
    [Class(Iri = "some:Offering")]
    public interface IProductOffering<T> : IService where T : IProduct
    {
        [Property(Iri = "some:oferredProduct")]
        T OfferedProduct { get; set; }

        [Collection(Iri = "some:texts")]
        ICollection<string> Texts { get; }
    }
}
