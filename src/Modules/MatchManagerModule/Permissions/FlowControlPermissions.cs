using System.ComponentModel;
using EvoSC.Common.Permissions.Attributes;

namespace EvoSC.Modules.Official.MatchManagerModule.Permissions;

[PermissionGroup]
public enum FlowControlPermissions
{
    [Description("Can restart the current map.")]
    RestartMatch,
    
    [Description("Can end the current round.")]
    EndRound,
    
    [Description("Can skip the current map and load the next one.")]
    SkipMap
}
