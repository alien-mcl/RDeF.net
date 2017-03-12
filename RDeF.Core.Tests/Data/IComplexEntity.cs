using System.Collections.Generic;
using RDeF.Entities;
using RDeF.Mapping.Attributes;

namespace RDeF.Data
{
    [Class("class1")]
    [Class("class2")]
    public interface IComplexEntity : IEntity
    {
        [Collection("ordinals")]
        ICollection<int> Ordinals { get; }

        [Collection("related")]
        ICollection<IComplexEntity> Related { get; }

        [Collection("floats")]
        IList<float> Floats { get; }

        [Collection("doubles")]
        IList<double> Doubles { get; }

        [Collection("other")]
        IList<IComplexEntity> Other { get; }
    }
}
