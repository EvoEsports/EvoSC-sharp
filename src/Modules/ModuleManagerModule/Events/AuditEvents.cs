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
    ModuleDisabled,
    
    /// <summary>
    /// Triggered when a module is reloaded by a user.
    /// </summary>
    [Identifier(Name = "ModuleManager:ModuleReloaded")]
    ModuleReloaded,
    
    /// <summary>
    /// Triggered when a module is loaded by a user.
    /// </summary>
    [Identifier(Name = "ModuleManager:ModuleLoaded")]
    ModuleLoaded,
    
    /// <summary>
    /// Triggered when a module is unloaded by a user.
    /// </summary>
    [Identifier(Name = "ModuleManager:ModuleUnloaded")]
    ModuleUnloaded,
    
    /// <summary>
    /// Triggered when a module is installed by a user.
    /// </summary>
    [Identifier(Name = "ModuleManager:ModuleInstalled")]
    ModuleInstalled,
    
    /// <summary>
    /// Triggered when a module is uninstalled by a user.
    /// </summary>
    [Identifier(Name = "ModuleManager:ModuleUninstalled")]
    ModuleUninstalled
}
