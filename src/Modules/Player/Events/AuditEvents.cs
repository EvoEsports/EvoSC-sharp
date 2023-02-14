using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Modules.Official.Player.Events;

public enum AuditEvents
{
    [Identifier(Name = "Players:PlayerKicked")]
    PlayerKicked,
    
    [Identifier(Name = "Players:PlayerMuted")]
    PlayerMuted,
    
    [Identifier(Name = "Players:PlayerUnmuted")]
    PlayerUnmuted,
    
    [Identifier(Name = "Players:PlayerBanned")]
    PlayerBanned,
    
    [Identifier(Name = "Players:PlayerUnbanned")]
    PlayerUnbanned,
    
    [Identifier(Name = "Players:PlayerUnblacklisted")]
    PlayerUnblacklisted,
}
