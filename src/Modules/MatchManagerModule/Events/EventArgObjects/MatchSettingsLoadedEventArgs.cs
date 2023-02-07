namespace EvoSC.Modules.Official.MatchManagerModule.Events.EventArgObjects;

public class MatchSettingsLoadedEventArgs : EventArgs
{
    /// <summary>
    /// The name of the match settings that wasd loaded.
    /// </summary>
    public required string Name { get; init; }
}
