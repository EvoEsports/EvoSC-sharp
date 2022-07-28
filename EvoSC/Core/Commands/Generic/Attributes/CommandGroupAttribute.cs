using System;
using EvoSC.Domain.Groups;

namespace EvoSC.Core.Commands.Generic.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class CommandGroupAttribute : Attribute
{
    public string Name { get; set; }
    public string? Description { get; set; }

    public CommandGroupAttribute(string name, string? description = null)
    {
        Name = name;
        Description = description;
    }
}
