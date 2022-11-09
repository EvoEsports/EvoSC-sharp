using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Common.Controllers.Context;

public class PlayerInteractionContext : GenericControllerContext
{
    public IPlayer Player { get; }

    public PlayerInteractionContext(IPlayer player)
    {
        Player = player;
    }


    public PlayerInteractionContext(IPlayer player, IControllerContext context) : base(context)
    {
        Player = player;
    }
}
