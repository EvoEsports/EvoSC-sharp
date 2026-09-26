namespace EvoSC.Modules.Official.TeamSettingsModule.Events;

/// <summary>
/// Exported so other modules can subscribe to the team settings events without a reference to
/// this module: both sides then agree on the same enum type, and therefore on the event names.
/// </summary>
[Export]
public enum TeamSettingsEvents
{
    /// <summary>
    /// Raised after team settings have been changed.
    /// </summary>
    SettingsUpdated
}
