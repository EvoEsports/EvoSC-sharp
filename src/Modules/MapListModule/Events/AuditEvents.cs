using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Modules.Official.MapListModule.Events;

public enum AuditEvents
{
    [Identifier(Name = "MapList:RemoveMapConfirm")]
    RemoveMapConfirm,
    
    [Identifier(Name = "MapList:RemoveMap")]
    RemoveMap,
}
