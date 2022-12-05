using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Middleware;
using EvoSC.Common.Middleware;
using EvoSC.Common.Remote.ChatRouter;

namespace EvoSC.Commands.Middleware;

public class HideCommandMiddleware
{
    private readonly ActionDelegate _next;
    public HideCommandMiddleware(ActionDelegate next)
    {
        _next = next;
    }

    public Task ExecuteAsync(ChatRouterPipelineContext context)
    {
        if (context.Args.MessageText.StartsWith("/"))
        {
            context.ForwardMessage = false;
        }
        
        return _next(context);
    }
}
