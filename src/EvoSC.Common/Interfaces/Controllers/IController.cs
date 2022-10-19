namespace EvoSC.Common.Interfaces.Controllers;

public interface IController : IDisposable
{
    /// <summary>
    /// Set the controller's context object.
    /// </summary>
    /// <param name="context"></param>
    public void SetContext(IControllerContext context);
    public IControllerContext GetContext();

    public event Action Disposed;
}
