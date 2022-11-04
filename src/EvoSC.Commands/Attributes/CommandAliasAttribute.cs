using EvoSC.Commands.Util;

namespace EvoSC.Commands.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class CommandAliasAttribute : Attribute
{
    public string Alias { get; }

    public CommandAliasAttribute(string alias)
    {
        Alias = alias;
    }
}
