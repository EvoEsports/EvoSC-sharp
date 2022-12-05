using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;

namespace EvoSC.Commands;

public class CommandAlias : ICommandAlias
{
    public string Name { get; init; }
    public object[] DefaultArgs { get; init; }
    public bool Hide { get; }

    public CommandAlias(string name, bool hide, params object[] args)
    {
        Name = name;
        DefaultArgs = args;
        Hide = hide;
    }

    public CommandAlias(CommandAliasAttribute attr)
    {
        Name = attr.Name;
        DefaultArgs = attr.Arguments;
        Hide = attr.Hide;
    }
}
