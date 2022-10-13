using EvoSC.Common.Config.Models;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using Microsoft.Extensions.DependencyInjection;

namespace EvoSC.Common.Controllers.Context;

public class GenericControllerContext : IControllerContext
{
    public IServiceScope ServiceScope { get; private set; }
    public IServerClient Server { get; }

    void IControllerContext.SetScope(IServiceScope scope)
    {
        ServiceScope = scope;
    }

    public GenericControllerContext(IServiceScope serviceScope, IServerClient serverClient)
    {
        ServiceScope = serviceScope;
        Server = serverClient;
    }
    
    public GenericControllerContext()
    {
    }

    public GenericControllerContext(IControllerContext context)
    {
        ServiceScope = context.ServiceScope;
        Server = context.Server;
    }
}
