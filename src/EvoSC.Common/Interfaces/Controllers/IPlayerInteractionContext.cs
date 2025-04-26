using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;

namespace EvoSC.Common.Interfaces.Controllers;

public interface IPlayerInteractionContext : IGenericControllerContext
{
    public IOnlinePlayer Player { get; }
    public IServerClient Server { get; }
    public IChatService Chat => Server.Chat;
}
