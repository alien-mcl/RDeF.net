using System.Collections.Generic;
using RDeF.Entities;
using RDeF.Mapping.Attributes;

namespace RDeF.Data
{
    [Class(Iri = "class1")]
    [Class(Iri = "class2")]
    public interface IComplexEntity : IEntity
    {
        [Collection(Iri = "ordinals")]
        ICollection<int> Ordinals { get; }

        [Collection(Iri = "related")]
        ICollection<IComplexEntity> Related { get; }

        [Collection(Iri = "floats")]
        IList<float> Floats { get; }

        [Collection(Iri = "doubles")]
        IList<double> Doubles { get; }

        [Collection(Iri = "other")]
        IList<IComplexEntity> Other { get; }
    }
}
