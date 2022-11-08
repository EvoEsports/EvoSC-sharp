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

    /// <summary>
    /// Find a command by it's name.
    /// </summary>
    /// <param name="alias"></param>
    /// <returns></returns>
    public IChatCommand FindCommand(string alias, bool withPrefix = true);
}
