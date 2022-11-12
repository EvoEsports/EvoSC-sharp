using System.Reflection;
using EvoSC.Commands.Interfaces;

namespace EvoSC.Commands;

public class ChatCommand : IChatCommand
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public string? Permission { get; init; }
    public Dictionary<string, ICommandAlias> Aliases { get; init; }
    public required Type ControllerType { get; init; }
    public required MethodInfo HandlerMethod { get; init; }
    public required ICommandParameter[] Parameters { get; init; }
    public bool UsePrefix { get; init; }
}
