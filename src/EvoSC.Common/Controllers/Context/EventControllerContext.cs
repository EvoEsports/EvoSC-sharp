using EvoSC.Common.Interfaces.Controllers;

namespace EvoSC.Common.Controllers.Context;

/// <summary>
/// Context that contains info about the fired event.
/// </summary>
public class EventControllerContext : GenericControllerContext, IEventControllerContext
{
    public EventControllerContext(IControllerContext context) : base(context.ServiceScope)
    {
    }
}
