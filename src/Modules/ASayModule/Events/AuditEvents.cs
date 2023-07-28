using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Modules.Official.ASayModule.Events;

public enum AuditEvents
{
    [Identifier(Name = "ASay:ShowAnnouncement")]
    ShowAnnouncement,
    
    [Identifier(Name = "ASay:ClearAnnouncement")]
    ClearAnnouncement,
}
