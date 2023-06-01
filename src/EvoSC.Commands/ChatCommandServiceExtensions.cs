using EvoSC.Commands.Interfaces;
using EvoSC.Commands.Middleware;
using EvoSC.Common.Interfaces.Middleware;
using EvoSC.Common.Middleware;
using SimpleInjector;

namespace EvoSC.Commands;

public static class ChatCommandServiceExtensions
{
    /// <summary>
    /// Add the command system to the service container.
    /// </summary>
    /// <param name="container"></param>
    /// <returns></returns>
    public static Container AddEvoScChatCommands(this Container container)
    {
        container.RegisterSingleton<IChatCommandManager, ChatCommandManager>();
        return container;
    }

    public static void UseEvoScCommands(this IActionPipelineManager pipelineManager, Container services)
    {
        pipelineManager.UseMiddleware<CommandsPermissionMiddleware>(PipelineType.ControllerAction, services);
        pipelineManager.UseMiddleware<CommandsMiddleware>(PipelineType.ChatRouter, services);
    }
}
