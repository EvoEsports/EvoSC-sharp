namespace EvoSC.Common.Interfaces.Controllers;

public interface IController : IDisposable
{
    /// <summary>
    /// The context contains information about the current action and easy access to
    /// internal resource.
    /// </summary>
    public IControllerContext Context { get; }
    
    /// <summary>
    /// Set the controller's context object.
    /// </summary>
    /// <param name="context"></param>
    public void SetContext(IControllerContext context);
}
