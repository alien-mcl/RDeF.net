using System;
using System.Text.RegularExpressions;

namespace RDeF.ComponentModel
{
    internal interface IContainer
    {
        void Register<TService>(Regex assemblyNamePattern = null);

        void Register<TService, TImplementation>() where TImplementation : TService;

        void Register<TService>(Type implementationType);

        void Register<TService>(TService instance);

        void Unregister<TService>();

        void Unregister<TService>(TService instance);

        TService Resolve<TService>();
    }
}
