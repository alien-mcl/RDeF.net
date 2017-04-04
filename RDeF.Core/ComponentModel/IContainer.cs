using System;
using System.Text.RegularExpressions;

namespace RDeF.ComponentModel
{
    internal interface IContainer : IComponentScope, IDisposable
    {
        void Register<TService>(Regex assemblyNamePattern = null);

        IComponentRegistration Register<TService, TImplementation>(Lifestyle lifestyle = Lifestyle.Singleton) where TImplementation : TService;

        IComponentRegistration Register<TService>(Type implementationType, Lifestyle lifestyle = Lifestyle.Singleton);

        IComponentRegistration Register<TService>(TService instance);

        void Unregister<TService>();

        void Unregister<TService>(TService instance);

        IContainer BeginScope();
    }
}
