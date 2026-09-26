namespace EvoSC.Modules.Official.SetNameModule.Events;

/// <summary>
/// Exported so other modules can react to nickname changes without a reference to this module:
/// both sides then agree on the same enum type, and therefore on the event name.
/// </summary>
[Export]
public enum SetNameEvents
{
    /// <summary>
    /// Triggered when a player changes their name.
    /// </summary>
    NicknameUpdated
}
