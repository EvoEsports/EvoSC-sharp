using System.ComponentModel;
using EvoSC.Common.Permissions.Attributes;

namespace EvoSC.Modules.Official.Player;

[PermissionGroup]
public enum ModPermissions
{
    [Description("Can kick players from the server.")]
    KickPlayer,
    
    [Description("Can b or blacklist players from the server.")]
    BanPlayer,
    
    [Description("Can mute the player from the chat.")]
    MutePlayer
}
