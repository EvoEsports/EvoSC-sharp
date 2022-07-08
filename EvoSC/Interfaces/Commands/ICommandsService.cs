using System;
using System.Reflection;
using System.Threading.Tasks;
using EvoSC.Domain.Commands;
using EvoSC.Domain.Players;

namespace EvoSC.Interfaces.Commands;

public interface ICommandsService
{
    public Task ClientOnPlayerChatCommand(DatabasePlayer databasePlayer, Command command);

    /// <summary>
    /// Register commands from a commands group's type.
    /// </summary>
    /// <param name="type"></param>
    public void RegisterCommands(Type type);
    /// <summary>
    /// Remove commands of a given commands group's type.
    /// </summary>
    /// <param name="type"></param>
    public void UnregisterCommands(Type type);
}
