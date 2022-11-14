using EvoSC.Common.Interfaces.Middleware;
using EvoSC.Modules.Attributes;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.ExampleModule;

[Module(Name = "ExampleModule", Description = "An example module to get people started.", IsInternal = true)]
public class ExampleModule : EvoScModule
{
    public ExampleModule(IActionPipeline pipeline, ILogger<ExampleModule> logger)
    {
        pipeline.AddComponent(next =>
        {
            return context =>
            {
                logger.LogInformation("hello from first registered middleware!");
                return next(context);
            };
        });
    }
}
