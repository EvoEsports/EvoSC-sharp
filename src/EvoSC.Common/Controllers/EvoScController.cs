using EvoSC.Common.Database.Models;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;

namespace EvoSC.Common.Controllers;

public abstract class EvoScController<TContext> : IController where TContext : IControllerContext
{
    public IControllerContext Context { get; private set; }

    void IController.SetContext(IControllerContext context)
    {
        Context = context;
    }

    public void Dispose()
    {
        // make sure to dispose of the service scope
        Context.ServiceScope.Dispose();
    }
}

public abstract class EvoScController : EvoScController<IControllerContext> {}
