using EvoSC.Commands;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Middleware;
using EvoSC.Common.Middleware;
using EvoSC.Common.Middleware.Attributes;
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

    public Task ExecuteAsync(IPipelineContext context)
    {
        _logger.LogInformation("Hello from middleware!");
        return _next(context);
    }
}
