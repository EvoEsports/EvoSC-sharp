using System.ComponentModel;
using EvoSC.Common.Permissions.Attributes;

namespace EvoSC.Modules.Official.ForceTeamModule.Permissions;

[PermissionGroup]
public enum ForceTeamPermissions
{
    [Description("[Permission.ForcePlayerTeam]")]
    ForcePlayerTeam,
    
    [Description("[Permission.AutoTeamBalance]")]
    AutoTeamBalance
}
