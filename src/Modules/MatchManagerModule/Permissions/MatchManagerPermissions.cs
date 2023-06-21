using System.ComponentModel;
using EvoSC.Common.Permissions.Attributes;

namespace EvoSC.Modules.Official.MatchManagerModule.Permissions;

[PermissionGroup]
public enum MatchManagerPermissions
{
    [Description("[Permission.SetLiveMode]")]
    SetLiveMode,
    
    [Description("[Permission.LoadMatchSettings]")]
    LoadMatchSettings
}
