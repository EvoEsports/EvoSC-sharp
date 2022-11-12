using EvoSC.Common.Events;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using Microsoft.Extensions.DependencyInjection;

namespace EvoSC.Common.Controllers.Context;

/// <summary>
/// Context that contains info about the fired event.
/// </summary>
public class EventControllerContext : GenericControllerContext
{
    public EventControllerContext(IControllerContext context) : base(context.ServiceScope)
    {
    }
}
