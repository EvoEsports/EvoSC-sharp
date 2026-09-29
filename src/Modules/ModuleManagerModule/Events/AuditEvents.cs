using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Modules.Official.ModuleManagerModule.Events;

public enum AuditEvents
{
    [Identifier(Name = "ModuleManager:ModuleEnabled")]
    ModuleEnabled,
    [Identifier(Name = "ModuleManager:ModuleDisabled")]
    ModuleDisabled,
    [Identifier(Name = "ModuleManager:ModuleReloaded")]
    ModuleReloaded,
    [Identifier(Name = "ModuleManager:ModuleLoaded")]
    ModuleLoaded,
    [Identifier(Name = "ModuleManager:ModuleUnloaded")]
    ModuleUnloaded,
    [Identifier(Name = "ModuleManager:ModuleInstalled")]
    ModuleInstalled,
    [Identifier(Name = "ModuleManager:ModuleUninstalled")]
    ModuleUninstalled
}