using System.Collections.Generic;
using RDeF.Entities;
using RDeF.Mapping.Attributes;

namespace RDeF.Data
{
    [Class(Iri = "some:class1")]
    [Class(Iri = "some:class2")]
    public interface IComplexEntity : IEntity
    {
        [Property(Iri = "some:name")]
        string Name { get; set; }

        [Collection(Iri = "some:ordinals")]
        ICollection<int> Ordinals { get; }

        [Collection(Iri = "some:related")]
        ICollection<IComplexEntity> Related { get; }

        [Collection(Iri = "some:floats")]
        IList<float> Floats { get; }

        [Collection(Iri = "some:doubles")]
        IList<double> Doubles { get; }

        [Collection(Iri = "some:other")]
        IList<IComplexEntity> Other { get; }
    }
}
