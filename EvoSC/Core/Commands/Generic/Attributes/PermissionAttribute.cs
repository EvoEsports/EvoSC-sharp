using System;

namespace EvoSC.Core.Commands.Generic.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class PermissionAttribute : Attribute
{
    public string Name { get; set; }

    public PermissionAttribute(string name)
    {
        Name = name;
    }
}
