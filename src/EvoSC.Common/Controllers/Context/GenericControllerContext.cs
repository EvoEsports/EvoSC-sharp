using EvoSC.Common.Interfaces.Controllers;
using SimpleInjector;

namespace EvoSC.Common.Controllers.Context;

/// <summary>
/// Generic context for any action within a controller. If you have multiple action types, you need to use
/// this.
/// </summary>
public class GenericControllerContext : IControllerContext
{
    public Scope ServiceScope { get; private set; }
    public IController Controller { get; init; }
    
    private readonly Dictionary<string, object> _customData = new();
    public Dictionary<string, object> CustomData => _customData;

    void IControllerContext.SetScope(Scope scope)
    {
        ServiceScope = scope;
    }

    public GenericControllerContext()
    {
        // default constructor, allows for custom context
    }

    public GenericControllerContext(Scope serviceScope)
    {
        ServiceScope = serviceScope;
    }

    public GenericControllerContext(IControllerContext context)
    {
        ServiceScope = context.ServiceScope;
        Controller = context.Controller;
    }
}
