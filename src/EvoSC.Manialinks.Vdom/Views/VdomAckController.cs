using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Util;
using EvoSC.Manialinks;
using EvoSC.Manialinks.Attributes;

namespace EvoSC.Manialinks.Vdom.Views;

/// <summary>
/// Receives the ack <c>Runtime/VdomRuntime.ms</c> sends via <c>TriggerPageAction</c> after
/// applying a patch -- routes through the existing <c>ManialinkActionManager</c> /
/// <c>ManialinkInteractionHandler</c> pipeline with no new infrastructure, exactly as the plan's
/// L4 calls for.
///
/// The route is <c>VdomAck/Ack/{viewName}/{seq}</c> -- explicit
/// <c>[ManialinkRoute(Route = "VdomAck")]</c> rather than the default class-name-derived route,
/// so <see cref="Runtime.RuntimeScriptBuilder.AckRoutePrefix"/> can hardcode it with no ambiguity.
/// </summary>
[Controller]
[ManialinkRoute(Route = "VdomAck")]
public class VdomAckController(IManialinkViewManager viewManager) : ManialinkController
{
    public Task AckAsync(string viewName, int seq)
    {
        viewManager.RecordAck(viewName, Context.Player.GetLogin(), seq);
        return Task.CompletedTask;
    }
}
