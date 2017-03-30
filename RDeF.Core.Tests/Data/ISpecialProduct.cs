using RDeF.Mapping.Attributes;

namespace RDeF.Data
{
    [Class(Iri = "SpecialProduct")]
    public interface ISpecialProduct : IProduct
    {
        [Property(Iri = "specialName")]
        string SpecialName { get; set; }
    }
}
