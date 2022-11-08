using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;

namespace EvoSC.Commands;

public class CommandAlias : ICommandAlias
{
    public string Name { get; init; }
    public object[] DefaultArgs { get; init; }

    public CommandAlias(string name, params object[] args)
    {
        Name = name;
        DefaultArgs = args;
    }

    public CommandAlias(CommandAliasAttribute attr)
    {
        Name = attr.Name;
        DefaultArgs = attr.Arguments;
    }
}
