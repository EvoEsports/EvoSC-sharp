using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Modules.Official.ModuleManagerModule.Events;

public enum AuditEvents
{
    /// <summary>
    /// Triggered when a module is enabled by a user.
    /// </summary>
    [Identifier(Name = "ModuleManager:ModuleEnabled")]
    ModuleEnabled,
    
    /// <summary>
    /// Triggered when a module is disabled by a user.
    /// </summary>
    [Identifier(Name = "ModuleManager:ModuleDisabled")]
    ModuleDisabled
}
