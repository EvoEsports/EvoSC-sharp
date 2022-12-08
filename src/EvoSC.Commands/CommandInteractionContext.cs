using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers.Context;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Commands;

public class CommandInteractionContext : PlayerInteractionContext
{
    /// <summary>
    /// The command that was executed in this context.
    /// </summary>
    public required IChatCommand CommandExecuted { get; init; }
    
    public CommandInteractionContext(IOnlinePlayer player, IControllerContext context) : base(player, context)
    {
    }
}
