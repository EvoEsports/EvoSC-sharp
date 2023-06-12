using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Modules.Official.ModuleManagerModule.Events;

public enum AuditEvents
{
    [Identifier(Name = "ModuleManager:ModuleEnabled")]
    ModuleEnabled,
    
    [Identifier(Name = "ModuleManager:ModuleDisabled")]
    ModuleDisabled
}
