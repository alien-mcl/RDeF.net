using RDeF.Entities;
using RDeF.Mapping.Attributes;

namespace RDeF.Data
{
    [Class(Iri = "some:Service")]
    public interface IService : IEntity
    {
        [Property(Iri = "some:image")]
        string Image { get; set; }
    }
}
