using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Remote.ChatRouter;

namespace EvoSC.Common.Interfaces;

public interface IRemoteChatRouter
{
    public Task SendMessageAsync(string message, IOnlinePlayer actor);
    public Task SendMessageAsync(ChatRouterPipelineContext pipelineContext);
}
