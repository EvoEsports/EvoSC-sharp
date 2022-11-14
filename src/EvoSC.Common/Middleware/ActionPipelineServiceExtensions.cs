using EvoSC.Common.Interfaces.Middleware;
using SimpleInjector;

namespace EvoSC.Common.Middleware;

public static class ActionPipelineServiceExtensions
{
    public static Container AddEvoScMiddlewarePipelines(this Container container)
    {
        // container.RegisterSingleton<IActionPipeline, ActionPipeline>();
        container.RegisterSingleton<IActionPipelineManager, ActionPipelineManager>();
        return container;
    }
}
