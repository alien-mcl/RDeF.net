namespace RDeF.ComponentModel
{
    /// <summary>Describes an abstract module.</summary>
    public interface IModule
    {
        /// <summary>Initializes the module.</summary>
        /// <param name="componentConfigurator">Component configurator that can be used for registrations.</param>
        void Initialize(IComponentConfigurator componentConfigurator);
    }
}
