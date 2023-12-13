namespace EvoSC.Commands.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class CommandAliasAttribute(string name, bool hide, params object[] args) : Attribute
{
    /// <summary>
    /// The name of the alias, which the player must type to call the command.
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    /// Default arguments for this alias, passed to the command.
    /// </summary>
    public object[] Arguments { get; } = args;

    /// <summary>
    /// Whether to hide the chat message that triggered this alias.
    /// </summary>
    public bool Hide { get; } = hide;

    public CommandAliasAttribute(string name, params object[] args) : this(name, false, args)
    {
    }
}
