using EvoSC.Commands;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Middleware;
using EvoSC.Common.Middleware;
using EvoSC.Common.Middleware.Attributes;
using EvoSC.Common.Remote.ChatRouter;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.ExampleModule;

[Middleware(For = PipelineType.ChatRouter)]
public class MyMiddleware
{
    private readonly ActionDelegate _next;
    private readonly ILogger<MyMiddleware> _logger;
    
    public MyMiddleware(ActionDelegate next, ILogger<MyMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public Task ExecuteAsync(ChatRouterPipelineContext context)
    {
        context.MessageText = context.MessageText.Replace("fuck", "f**k");
        
        return _next(context);
    }
}
