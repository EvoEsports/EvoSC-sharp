using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;

namespace EvoSC.Commands;

public class CommandAlias(string name, bool hide, params object[] args) : ICommandAlias
{
    public string Name { get; init; } = name;
    public object[] DefaultArgs { get; init; } = args;
    public bool Hide { get; } = hide;

    public CommandAlias(CommandAliasAttribute attr) : this(attr.Name, attr.Hide, attr.Arguments)
    {
    }
}
