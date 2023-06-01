using EvoSC.Common.Interfaces.Controllers;

namespace EvoSC.Common.Controllers;

public abstract class EvoScController<TContext> : IController where TContext : class, IControllerContext
{
    /// <summary>
    /// Information related to the current action and data assigned for the action's context.
    /// </summary>
    public TContext Context { get; private set; }

    void IController.SetContext(IControllerContext context)
    {
        Context = (TContext)context;
    }

    public IControllerContext GetContext() => Context;

    public event Action? Disposed;

    public void Dispose()
    {
        // make sure to dispose of the service scope
        Context.ServiceScope.Dispose(); 
        Disposed?.Invoke();
        GC.SuppressFinalize(this);
    }
}
