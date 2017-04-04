﻿using System;

namespace RDeF.ComponentModel
{
    internal interface IComponentRegistration
    {
        Type ServiceType { get; }

        Type ImplementationType { get; }

        object Instance { get; }

        Action<IContainer, object> OnActivate { get; set; }
    }
}
