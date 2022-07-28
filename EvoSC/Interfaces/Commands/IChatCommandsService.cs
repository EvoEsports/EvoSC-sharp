
using System.Threading.Tasks;
using EvoSC.Interfaces.Messages;
using EvoSC.Interfaces.Commands;

public interface IChatCommandsService : ICommandsService
{
    public Task OnChatMessage(IServerServerChatMessage message);
}
