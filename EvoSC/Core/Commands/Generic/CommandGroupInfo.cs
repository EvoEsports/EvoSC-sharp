using EvoSC.Core.Commands.Generic.Interfaces;

namespace EvoSC.Core.Commands.Generic;

public class CommandGroupInfo : ICommandGroupInfo
{
    public string Name { get; }
    public string Description { get; }
    public string? Permission { get; set; }

    public CommandGroupInfo(string name, string description, string? permission = null)
    {
        Name = name;
        Description = description;
        Permission = permission;
    }
}
