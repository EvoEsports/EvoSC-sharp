using EvoSC.Commands.Util;

namespace EvoSC.Commands.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class CommandAliasAttribute : Attribute
{
    public string Name { get; }
    public object[] Arguments { get; }

    public CommandAliasAttribute(string name, params object[] args)
    {
        Name = name;
        Arguments = args;
    }
}
