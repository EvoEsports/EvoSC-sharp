using System.ComponentModel;
using EvoSC.Common.Permissions.Attributes;

namespace EvoSC.Modules.Official.MatchManagerModule.Permissions;

[PermissionGroup]
public enum MatchControlPermissions
{
    [Description("[Permission.RestartMatch]")]
    RestartMatch,
    
    [Description("[Permission.EndRound]")]
    EndRound,
    
    [Description("[Permission.SkipMap]")]
    SkipMap,
    
    [Description("[Permission.StartMatch]")]
    StartMatch,
    
    [Description("[Permission.EndMatch]")]
    EndMatch,
    
    [Description("[Permission.SetTeamPoints]")]
    SetTeamPoints,
    
    [Description("[Permission.PauseMatch]")]
    PauseMatch
}
