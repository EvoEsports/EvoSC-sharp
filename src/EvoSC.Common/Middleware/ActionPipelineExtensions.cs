using EvoSC.Common.Interfaces.Middleware;
using SimpleInjector;

namespace EvoSC.Common.Middleware;

public static class ActionPipelineExtensions
{
    /// <summary>
    /// Add the acton pipeline manager which manages the application's middlewares.
    /// </summary>
    /// <param name="container"></param>
    /// <returns></returns>
    public static Container AddEvoScMiddlewarePipelines(this Container container)
    {
        container.RegisterSingleton<IActionPipelineManager, ActionPipelineManager>();
        return container;
    }
}
