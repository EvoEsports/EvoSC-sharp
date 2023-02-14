using EvoSC.Common.Interfaces.Middleware;
using EvoSC.Common.Util.Auditing;
using SimpleInjector;

namespace EvoSC.Common.Interfaces.Controllers;

public interface IControllerContext : IPipelineContext
{
    /// <summary>
    /// The service scope used to create this context.
    /// </summary>
    public Scope ServiceScope { get; }
    
    /// <summary>
    /// Pointer to the controller instance for this context.
    /// </summary>
    public IController Controller { get; }

    /// <summary>
    /// The audit associated with the event of this context.
    /// </summary>
    public AuditEventBuilder AuditEvent { get; }
    
    /// <summary>
    /// This is any data that can be set to be included with the context.
    /// This is typically set in middlewares. Obviously, if the context supports
    /// a type of data already, use that instead.
    /// </summary>
    Dictionary<string, object> CustomData { get; }

    /// <summary>
    /// Set the service scope for this context.
    /// </summary>
    /// <param name="scope">Service scope to set for this context.</param>
    public void SetScope(Scope scope);
}
