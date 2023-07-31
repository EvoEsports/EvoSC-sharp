using System.ComponentModel;
using EvoSC.Common.Permissions.Attributes;

namespace EvoSC.Modules.Official.Player;

[PermissionGroup]
public enum ModPermissions
{
    [Description("[Permission.KickPlayer]")]
    KickPlayer,
    
    [Description("[Permission.BanPlayer]")]
    BanPlayer,
    
    [Description("[Permission.MutePlayer]")]
    MutePlayer
}
