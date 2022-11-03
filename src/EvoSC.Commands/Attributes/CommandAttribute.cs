using System.Text.RegularExpressions;

namespace EvoSC.Commands.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class CommandAttribute : Attribute
{
    /// <summary>
    /// Name of the command.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Summary/Description of the command.
    /// </summary>
    public string Description { get; }

    public CommandAttribute(string name, string description)
    {
        if (!Regex.IsMatch(name, "[\\w\\d]+"))
        {
            throw new ArgumentException("Command name must be alphanumeric", "name");
        }

        if (description.Trim() == string.Empty)
        {
            throw new ArgumentException("Command description cannot be empty.", "description");
        }

        Name = name;
        Description = description;
    }
}

