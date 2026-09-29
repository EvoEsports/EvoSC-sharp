using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Modules.Official.MapsModule.Events;

/// <summary>
/// Exported so other modules can audit map changes without a reference to this module: both
/// sides then agree on the same enum type, and therefore on the audit event name.
/// </summary>
[Export]
public enum AuditEvents
{
    [Identifier(Name = "Maps:MapAdded")]
    MapAdded,
    
    [Identifier(Name = "Maps:MapRemoved")]
    MapRemoved
}
