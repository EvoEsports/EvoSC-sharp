using EvoSC.Commands.Interfaces;
using SimpleInjector;

namespace EvoSC.Commands;

public static class ChatCommandServiceExtensions
{
    public static Container AddEvoScChatCommands(this Container container)
    {
        container.RegisterSingleton<IChatCommandManager, ChatCommandManager>();
        container.RegisterSingleton<ICommandInteractionHandler, CommandInteractionHandler>();
        return container;
    }
}
