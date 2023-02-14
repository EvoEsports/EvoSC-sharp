using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Common.Controllers.Context;

/// <summary>
/// Context for any action that was made by a player through one of the interaction systems. For example
/// commands or Manialinks.
/// </summary>
public class PlayerInteractionContext : GenericControllerContext
{
    /// <summary>
    /// The player that executed the action.
    /// </summary>
    public IOnlinePlayer Player { get; }

    public PlayerInteractionContext(IOnlinePlayer player, IControllerContext context) : base(context)
    {
        Player = player;
        AuditEvent.CausedBy(player);
    }
}
