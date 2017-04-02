using System;
using System.Text.RegularExpressions;

namespace RDeF.ComponentModel
{
    internal interface IContainer : IDisposable
    {
        void Register<TService>(Regex assemblyNamePattern = null);

        void Register<TService, TImplementation>(Lifestyle lifestyle = Lifestyle.Singleton) where TImplementation : TService;

        void Register<TService>(Type implementationType, Lifestyle lifestyle = Lifestyle.Singleton);

        void Register<TService>(TService instance);

        void Unregister<TService>();

        void Unregister<TService>(TService instance);

        bool IsRegistered<TService>();

        IContainer BeginScope();

        TService Resolve<TService>();

        object Resolve(Type type);
    }
}
