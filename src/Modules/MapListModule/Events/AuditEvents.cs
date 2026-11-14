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

    /// <summary>
    /// Triggered when a map is disabled.
    /// </summary>
    [Identifier(Name = "MapList:DisableMap")]
    DisableMap,

    /// <summary>
    /// Triggered when a map is enabled.
    /// </summary>
    [Identifier(Name = "MapList:EnableMap")]
    EnableMap,

    /// <summary>
    /// Triggered when a tag is added to a map.
    /// </summary>
    [Identifier(Name = "MapList:AddTag")]
    AddTag,

    /// <summary>
    /// Triggered when a tag is removed from a map.
    /// </summary>
    [Identifier(Name = "MapList:RemoveTag")]
    RemoveTag,

    /// <summary>
    /// Triggered when a map is added from Trackmania Exchange through the map list.
    /// </summary>
    [Identifier(Name = "MapList:AddTmxMap")]
    AddTmxMap,
}
