using System.Reflection;

namespace EvoSC.Commands.Interfaces;

public interface IChatCommand
{
    /// <summary>
    /// The name of the command.
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// Description or summary of what the command is or do.
    /// </summary>
    public string Description { get; }
    /// <summary>
    /// The permission required by the user to execute the command.
    /// </summary>
    public string? Permission { get; }
    /// <summary>
    /// Alternative names or keywords that can be used to execute the command. Aliases can be used to execute
    /// commands with default arguments. If no default arguments are provided for the alias, the user must provide
    /// all required arguments.
    /// </summary>
    public Dictionary<string, ICommandAlias> Aliases { get; }
    /// <summary>
    /// The type of the controller class where the command method lies within.
    /// </summary>
    public Type ControllerType { get; }
    /// <summary>
    /// The callback method that is used to execute the command.
    /// </summary>
    public MethodInfo HandlerMethod { get; }
    /// <summary>
    /// List of parameters that the command requires as arguments.
    /// </summary>
    public ICommandParameter[] Parameters { get; }
    /// <summary>
    /// Whether to prefix the command's name with the command prefix (usually '/') when registering the command
    /// or looking it up.
    /// </summary>
    public bool UsePrefix { get; }
}
