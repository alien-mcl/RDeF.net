namespace RDeF.ComponentModel
{
    /// <summary>Describes an abstract scope from whicn components can be resolved.</summary>
    public interface IComponentScope
    {
        /// <summary>Resolves an instance of the <typeparamref name="TService" />.</summary>
        /// <typeparam name="TService">Type of the service to resolve.</typeparam>
        /// <returns>Instance of the <typeparamref name="TService" />.</returns>
        TService Resolve<TService>();
    }
}
