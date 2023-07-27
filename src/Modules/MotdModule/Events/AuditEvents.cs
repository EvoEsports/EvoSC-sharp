using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Modules.Official.MotdModule.Events;

public enum AuditEvents
{
    [Identifier(Name = "MotdModule:IntervalSet")]
    IntervalSet,
    [Identifier(Name = "MotdModule:UrlSet")]
    UrlSet,
    [Identifier(Name = "MotdModule:LocalTextSet")]
    LocalTextSet,
    [Identifier(Name = "MotdModule:LocalTextEditOpened")]
    LocalTextEditOpened,
    [Identifier(Name = "MotdModule:IsLocalSet")]
    IsLocalSet
}
