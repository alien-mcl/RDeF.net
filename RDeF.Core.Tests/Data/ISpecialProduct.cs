using RDeF.Mapping.Attributes;

namespace RDeF.Data
{
    [Class(Iri = "some:some:SpecialProduct")]
    public interface ISpecialProduct : IProduct
    {
        [Property(Iri = "specialName")]
        string SpecialName { get; set; }
    }
}
