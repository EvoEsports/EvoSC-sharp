using EvoSC.Common.Events.CoreEvents;
using EvoSC.Common.Interfaces.Middleware;

namespace EvoSC.Common.Remote.ChatRouter;

public class ChatRouterPipelineContext : IPipelineContext
{
    public required bool ForwardMessage { get; set; }
    public required ChatMessageEventArgs Args { get; init; }
}
