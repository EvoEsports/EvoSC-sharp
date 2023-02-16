using EvoSC.Common.Controllers.Context;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Manialinks;

public class ManialinkInteractionContext : PlayerInteractionContext
{
    public ManialinkInteractionContext(IOnlinePlayer player, IControllerContext context) : base(player, context)
    {
    }
}
