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

    public ChatCommandAttribute(string name, string description, string? permission=null)
    {
        CommandUtils.ValidateCommandName(name);

        if (description.Trim() == string.Empty)
        {
            throw new ArgumentException("Command description cannot be empty.", "description");
        }

        Name = name;
        Description = description;
        Permission = permission;
    }
}

