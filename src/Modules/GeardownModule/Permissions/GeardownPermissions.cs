using System.ComponentModel;
using EvoSC.Common.Permissions.Attributes;

namespace EvoSC.Modules.Evo.GeardownModule.Permissions;

[PermissionGroup]
public enum GeardownPermissions
{
    [Description("Can start a match through geardown.")]
    StartMatch
}
