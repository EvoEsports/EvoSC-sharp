using EvoSC.Commands.Interfaces;
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
        container.RegisterSingleton<ICommandInteractionHandler, CommandInteractionHandler>();
        return container;
    }
}
