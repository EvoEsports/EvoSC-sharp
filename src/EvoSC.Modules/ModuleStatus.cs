namespace EvoSC.Modules;

/// <summary>
/// Lifecycle status of a loaded module.
/// </summary>
public enum ModuleStatus
{
    /// <summary>
    /// The module has been loaded but is not enabled.
    /// </summary>
    Loaded,

    /// <summary>
    /// The module is enabled and its features are active.
    /// </summary>
    Enabled,

    /// <summary>
    /// The module was disabled after being enabled.
    /// </summary>
    Disabled,

    /// <summary>
    /// The module failed during load, enable or disable.
    /// </summary>
    Error
}