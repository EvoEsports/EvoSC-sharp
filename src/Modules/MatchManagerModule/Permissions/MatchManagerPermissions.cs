using System.ComponentModel;
using EvoSC.Common.Permissions.Attributes;

namespace EvoSC.Modules.Official.MatchManagerModule.Permissions;

[PermissionGroup]
public enum MatchManagerPermissions
{
    [Description("Can set the current live mode.")]
    SetLiveMode,
    
    [Description("Can load match settings files.")]
    LoadMatchSettings
}
