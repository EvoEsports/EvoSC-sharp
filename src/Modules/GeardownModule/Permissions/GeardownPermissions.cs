using System.ComponentModel;
using EvoSC.Common.Permissions.Attributes;

namespace EvoSC.Modules.Evo.GeardownModule.Permissions;

[PermissionGroup]
public enum GeardownPermissions
{
    [Description("Can start a match through geardown.")]
    StartMatch,
    
    [Description("Can set up the server for a match.")]
    SetupMatch,
    
    [Description("Can set custom points for a player.")]
    SetPoints,
    
    [Description("Can pause/unpause a match.")]
    PauseMatch,
    
    [Description("Allows to manage the server name.")]
    ServerName
}
