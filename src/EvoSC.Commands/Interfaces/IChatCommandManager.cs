using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Parsing;

namespace EvoSC.Commands.Interfaces;

public interface IChatCommandManager : IControllerActionRegistry
{
    /// <summary>
    /// The value reader for command parameters.
    /// </summary>
    public IValueReaderManager ValueReader { get; }
    
    /// <summary>
    /// Register a new chat command.
    /// </summary>
    /// <param name="cmd">An instance containing registration info of a command.</param>
    public void AddCommand(IChatCommand cmd);
    /// <summary>
    /// Register a new chat command with a builder action.
    /// </summary>
    /// <param name="builder">An action or lambda that provides the command builder.</param>
    public IChatCommand AddCommand(Action<ChatCommandBuilder> builder);

    public void RemoveCommand(IChatCommand cmd);
    /// <summary>
    /// Find a command by it's name.
    /// </summary>
    /// <param name="alias">Name or alias of the command to find.</param>
    /// <returns></returns>
    public IChatCommand FindCommand(string alias);
    /// <summary>
    /// Find a command by it's name.
    /// </summary>
    /// <param name="alias">Name or alias of the command to find.</param>
    /// <param name="withPrefix">Whether to include prefix in the name.</param>
    /// <returns></returns>
    public IChatCommand FindCommand(string alias, bool withPrefix);
}
