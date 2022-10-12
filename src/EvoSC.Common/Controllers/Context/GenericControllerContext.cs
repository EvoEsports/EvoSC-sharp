using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace EvoSC.Common.Controllers.Context;

public class GenericControllerContext : IControllerContext
{
    public IServiceScope ServiceScope { get; private set; }
    
    void IControllerContext.SetScope(IServiceScope scope)
    {
        ServiceScope = scope;
    }

    public GenericControllerContext(IServiceScope serviceScope)
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
