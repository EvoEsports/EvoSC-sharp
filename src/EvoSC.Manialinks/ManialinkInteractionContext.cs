using EvoSC.Common.Controllers.Context;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Manialinks.Interfaces.Models;

namespace EvoSC.Manialinks;

public class ManialinkInteractionContext
    (IOnlinePlayer player, IServerClient server, IControllerContext context) : PlayerInteractionContext(player, server, context),
        IManialinkInteractionContext
{
    public required IManialinkActionContext ManialinkAction { get; init; }
    public required IManialinkManager ManialinkManager { get; init; }
}
