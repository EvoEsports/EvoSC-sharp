namespace EvoSC.Modules.Official.MatchManagerModule.Events.EventArgObjects;

public class LiveModeSetEventArgs : EventArgs
{
    /// <summary>
    /// The name alias for the mode.
    /// </summary>
    public required string ModeAlias { get; init; }
    
    /// <summary>
    /// The full name of the mode.
    /// </summary>
    public required string ModeName { get; init; }
}
