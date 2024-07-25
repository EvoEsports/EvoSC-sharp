using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers.Context;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Commands;

public class CommandInteractionContext
    (IOnlinePlayer player, IServerClient server, IControllerContext context) : PlayerInteractionContext(player, server, context),
        ICommandInteractionContext
{
    public required IChatCommand CommandExecuted { get; init; }
}
