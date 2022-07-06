using System;

namespace EvoSC.Attributes;

public class PermissionAttribute : Attribute
{
    public PermissionAttribute(string permissionName)
    {
        PermissionName = permissionName;
    }

    public string PermissionName { get; }
}
