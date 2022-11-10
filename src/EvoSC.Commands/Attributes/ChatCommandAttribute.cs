using System.Text.RegularExpressions;
using EvoSC.Commands.Util;

namespace EvoSC.Commands.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ChatCommandAttribute : Attribute
{
    /// <summary>
    /// Name of the command.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Summary/Description of the command.
    /// </summary>
    public string Description { get; }
    
    /// <summary>
    /// The permission required to execute this command.
    /// </summary>
    public string? Permission { get; }
    /// <summary>
    /// Do not automatically prefix the command with the command prefix.
    /// </summary>
    public bool UsePrefix { get; }

    public ChatCommandAttribute(string name, string description, string? permission, bool usePrefix)
    {
        if (description.Trim() == string.Empty)
        {
            throw new ArgumentException("Command description cannot be empty.", "description");
        }
        
        if (name.Trim() == string.Empty)
        {
            throw new ArgumentException("Command name cannot be empty.", "name");
        }

        Name = name;
        Description = description;
        Permission = permission;
        UsePrefix = usePrefix;
    }

    public ChatCommandAttribute(string name, string description, string permission) : this(name, description,
        permission, true)
    {
    }

    public ChatCommandAttribute(string name, string description) : this(name, description, null, true)
    {
    }
}

