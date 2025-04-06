namespace EvoSC.Common.Events.CoreEvents;

/// <summary>
/// Events for changes to match settings.
/// </summary>
public enum MatchSettingsEvent
{
    /// <summary>
    /// Triggered when the script settings has been changed on the server.
    /// </summary>
    ScriptSettingsChanged,
    
    /// <summary>
    /// Triggered when a matchsettings is loaded.
    /// </summary>
    MatchSettingsLoaded,
    
    /// <summary>
    /// Triggered when a new matchsettings has been created.
    /// </summary>
    MatchSettingsCreated,
    
    /// <summary>
    /// Triggered when an existing matchsettings was updated.
    /// </summary>
    MatchSettingsUpdated,
    
    /// <summary>
    /// triggered when a matchsettings has been deleted.
    /// </summary>
    MatchSettingsDeleted
}
