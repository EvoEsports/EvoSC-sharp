using EvoSC.Common.Remote;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;

namespace EvoSC.Common.Interfaces.Controllers;

public interface IControllerContext
{
    /// <summary>
    /// The service scope used to create this context.
    /// </summary>
    public Scope ServiceScope { get; }

    /// <summary>
    /// Set the service scope for this context.
    /// </summary>
    /// <param name="scope"></param>
    public void SetScope(Scope scope);
}
