namespace EvoSC.Common.Interfaces.Controllers;

public interface IController : IDisposable
{
    /// <summary>
    /// Set the controller's context object.
    /// </summary>
    /// <param name="context">The context of the action that was executed.</param>
    public void SetContext(IControllerContext context);
    /// <summary>
    /// Get the action context of this controller instance.
    /// </summary>
    /// <returns></returns>
    public IControllerContext GetContext();

    /// <summary>
    /// This event is called when the controller is disposed.
    /// </summary>
    public event Action Disposed;
}
