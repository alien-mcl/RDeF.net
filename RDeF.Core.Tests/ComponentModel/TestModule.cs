using RDeF.Mapping.Converters;

namespace RDeF.ComponentModel
{
    public class TestModule : IModule
    {
        public void Initialize(IComponentConfigurator componentConfigurator)
        {
            componentConfigurator.WithConverter<TestConverter>();
        }
    }
}
