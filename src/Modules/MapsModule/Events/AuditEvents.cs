using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Modules.Official.MapsModule.Events;

public enum AuditEvents
{
    [Identifier(Name = "Maps:MapAdded")]
    MapAdded,
    
    [Identifier(Name = "Maps:MapRemoved")]
    MapRemoved
}
