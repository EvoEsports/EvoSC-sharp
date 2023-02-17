using EvoSC.Common.Controllers.Context;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Manialinks.Interfaces.Models;

namespace EvoSC.Manialinks;

public class ManialinkInteractionContext : PlayerInteractionContext
{
    public IManialinkAction ManialinkActionExecuted { get; init; }
    
    public ManialinkInteractionContext(IOnlinePlayer player, IControllerContext context) : base(player, context)
    {
    }
}
