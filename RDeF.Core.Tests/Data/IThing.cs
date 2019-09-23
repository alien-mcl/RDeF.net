using RDeF.Entities;
using RDeF.Mapping.Attributes;

namespace RDeF.Data
{
    [Class(Iri = "some:Thing")]
    public interface IThing : IEntity
    {
        [Property(Iri = "some:abstract")]
        string Abstract { get; set; }
    }
}
