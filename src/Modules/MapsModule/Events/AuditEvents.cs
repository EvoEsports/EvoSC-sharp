using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Modules.Official.Maps.Events;

public enum AuditEvents
{
    [Identifier(Name = "Maps:MapAdded")]
    MapAdded,
    
    [Identifier(Name = "Maps:MapRemoved")]
    MapRemoved
}
