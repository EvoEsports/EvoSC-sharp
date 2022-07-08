
using System.Threading.Tasks;
using EvoSC.Interfaces.Chat;
using EvoSC.Interfaces.Commands;

public interface IChatCommandsService : ICommandsService
{
    public Task OnChatMessage(IServerServerChatMessage message);
}
