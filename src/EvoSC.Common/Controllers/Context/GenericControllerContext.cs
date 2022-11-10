using EvoSC.Common.Config.Models;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;

namespace EvoSC.Common.Controllers.Context;

/// <summary>
/// Generic context for any action within a controller. If you have multiple action types, you need to use
/// this.
/// </summary>
public class GenericControllerContext : IControllerContext
{
    public Scope ServiceScope { get; private set; }

    void IControllerContext.SetScope(Scope scope)
    {
        ServiceScope = scope;
    }

    public GenericControllerContext(Scope serviceScope)
    {
        ServiceScope = serviceScope;
    }
    
    public GenericControllerContext()
    {
    }

    public GenericControllerContext(IControllerContext context)
    {
        ServiceScope = context.ServiceScope;
    }
}
