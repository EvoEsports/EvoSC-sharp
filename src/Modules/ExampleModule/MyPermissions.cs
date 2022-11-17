using System.ComponentModel;
using EvoSC.Common.Permissions.Attributes;
using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Modules.Official.ExampleModule;

/// <summary>
/// These are my permissions.
/// </summary>
[PermissionGroup]
[Identifier(Name = "myPermissions")]
public enum MyPermissions
{
    [Identifier(Name = "myPerm1"), Description("This is my first permission.")]
    MyPerm1,
    [Identifier(Name = "myPerm2"), Description("This is my second permission.")]
    MyPerm2,
    [Identifier(Name = "myPerm3"), Description("This is my third permission.")]
    MyPerm3
}
