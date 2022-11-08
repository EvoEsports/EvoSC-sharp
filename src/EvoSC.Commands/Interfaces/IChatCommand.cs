using System.Reflection;

namespace EvoSC.Commands.Interfaces;

public interface IChatCommand
{
    public string Name { get; }
    public string Description { get; }
    public string? Permission { get; }
    public IEnumerable<string> Aliases { get; }
    public Type ControllerType { get; }
    public MethodInfo HandlerMethod { get; }
    public ICommandParameter[] Parameters { get; }
    public bool UsePrefix { get; }
}
