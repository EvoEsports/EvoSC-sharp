using System.Diagnostics.Metrics;
using EvoSC.Commands.Util;

namespace EvoSC.Commands.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class CommandAliasAttribute : Attribute
{
    public string Name { get; }
    public object[] Arguments { get; }
    public bool Hide { get; }

    public CommandAliasAttribute(string name, bool hide, params object[] args)
    {
        Name = name;
        Arguments = args;
        Hide = hide;
    }
    
    public CommandAliasAttribute(string name, params object[] args) : this(name, false, args)
    {
    }
}
