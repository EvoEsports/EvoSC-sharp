using EvoSC.Common.Util.Auditing;
using SimpleInjector;

namespace EvoSC.Common.Interfaces.Controllers;

public interface IContextService
{
    /// <summary>
    /// Create a new context for a scope.
    /// </summary>
    /// <param name="scope">The scope to create a context in.</param>
    /// <param name="controller">The controller to create the context for.</param>
    /// <returns></returns>
    internal IControllerContext CreateContext(Scope scope, IController controller);
    
    /// <summary>
    /// Get the current context in the current scope.
    /// </summary>
    /// <returns></returns>
    public IControllerContext GetContext();

    /// <summary>
    /// Begin auditing a new event.
    /// </summary>
    /// <returns></returns>
    public AuditEventBuilder Audit();
}
