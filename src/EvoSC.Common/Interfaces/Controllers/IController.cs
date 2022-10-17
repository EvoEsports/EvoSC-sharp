namespace EvoSC.Common.Interfaces.Controllers;

public interface IController<TContext> : IDisposable where TContext : IControllerContext
{
    /// <summary>
    /// The context contains information about the current action and easy access to
    /// internal resource.
    /// </summary>
    public TContext Context { get; }
    
    /// <summary>
    /// Set the controller's context object.
    /// </summary>
    /// <param name="context"></param>
    public void SetContext(TContext context);
}

public interface IController : IController<IControllerContext> {}
