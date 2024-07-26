using System.ComponentModel;
using EvoSC.Common.Permissions.Attributes;

namespace EvoSC.Modules.Official.TeamSettingsModule.Permissions;

[PermissionGroup]
public enum TeamSettingsPermissions
{
    [Description("[Permission.EditTeamSettings]")]
    EditTeamSettings
}
