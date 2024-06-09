using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Modules.Official.MapListModule.Events;

public enum AuditEvents
{
    /// <summary>
    /// Triggered when a player tries to confirm map deletion.
    /// </summary>
    [Identifier(Name = "MapList:RemoveMapConfirm")]
    RemoveMapConfirm,
    
    /// <summary>
    /// Triggered when a map is removed.
    /// </summary>
    [Identifier(Name = "MapList:RemoveMap")]
    RemoveMap,
}
