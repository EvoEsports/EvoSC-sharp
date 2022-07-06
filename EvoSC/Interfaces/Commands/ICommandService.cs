using System.Threading.Tasks;
using EvoSC.Domain.Commands;
using EvoSC.Domain.Players;

namespace EvoSC.Interfaces.Commands;

public interface ICommandService
{
    public Task ClientOnPlayerChatCommand(Player player, Command command);
}
