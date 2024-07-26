using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Common.Interfaces.Controllers;

public interface IPlayerInteractionContext : IGenericControllerContext
{
    public IOnlinePlayer Player { get; }
    public IServerClient Server { get; }
}
