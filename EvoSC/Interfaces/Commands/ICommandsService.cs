using System;
using System.Reflection;
using System.Threading.Tasks;
using EvoSC.Core.Commands.Generic;
using EvoSC.Core.Commands.Generic.Interfaces;
using EvoSC.Domain.Players;
using EvoSC.Interfaces.Chat;
using EvoSC.Interfaces.Players;

namespace EvoSC.Interfaces.Commands;

public interface ICommandsService
{
    /// <summary>
    /// Register commands from a commands group's type.
    /// </summary>
    /// <param name="type"></param>
    public void RegisterCommands(Type type);
    /// <summary>
    /// Register commands from a commands group's type.
    /// </summary>
    /// <param name="type"></param>
    public void RegisterCommands<T>();
    /// <summary>
    /// Remove commands of a given commands group's type.
    /// </summary>
    /// <param name="type"></param>
    public void UnregisterCommands(Type type);
    /// <summary>
    /// Execute a command based on the parser result.
    /// </summary>
    /// <param name="parserResult"></param>s
    /// <returns></returns>
    public Task<ICommandResult> ExecuteCommand(ICommandContext context, ICommandParserResult parserResult);
    /// <summary>
    /// Get a command from it's name.
    /// </summary>
    /// <param name="group"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public ICommand GetCommand(string group, string name);
}
