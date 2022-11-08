using System.Reflection;
using EvoSC.Commands.Interfaces;

namespace EvoSC.Commands;

public class ChatCommand : IChatCommand
{
    public string Name { get; init; }
    public string Description { get; init; }
    public string? Permission { get; init; }
    public Dictionary<string, ICommandAlias> Aliases { get; init; }
    public Type ControllerType { get; init; }
    public MethodInfo HandlerMethod { get; init; }
    public ICommandParameter[] Parameters { get; init; }
    public bool UsePrefix { get; init; }
}
