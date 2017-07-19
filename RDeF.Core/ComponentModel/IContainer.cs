using System;
using System.Text.RegularExpressions;

namespace RDeF.ComponentModel
{
    internal interface IContainer : IComponentScope, IDisposable
    {
        void Register<TService>(Regex assemblyNamePattern = null);

        IComponentRegistration Register<TService, TImplementation>(Lifestyle lifestyle = Lifestyle.Singleton) where TImplementation : TService;

        IComponentRegistration Register<TService>(Type implementationType, Lifestyle lifestyle = Lifestyle.Singleton);

        IComponentRegistration Register<TService, TImplementation>(string name, Lifestyle lifestyle = Lifestyle.Singleton) where TImplementation : TService;

        IComponentRegistration Register<TService>(Type implementationType, string name, Lifestyle lifestyle = Lifestyle.Singleton);

        IComponentRegistration Register<TService>(TService instance, string name = null);

        IContainer BeginScope();
    }
}
