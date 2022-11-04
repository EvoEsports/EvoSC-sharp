using EvoSC.Common.Interfaces.Controllers;

namespace EvoSC.Commands.Interfaces;

public interface IChatCommandManager : IControllerActionRegistry
{
    /// <summary>
    /// Register a new chat command.
    /// </summary>
    /// <param name="cmd"></param>
    public void AddCommand(IChatCommand cmd);
    /// <summary>
    /// Register a new chat command with a builder action.
    /// </summary>
    /// <param name="builder"></param>
    public void AddCommand(Action<ChatCommandBuilder> builder);
}
