using System;
using GameHost.V3.Injection;

namespace EvoSC.Core.Plugins
{
    public abstract class CustomInjectorAttribute : Attribute
    {
        public abstract void Invoke(IReadOnlyContext context, object system, object member);
    }
}